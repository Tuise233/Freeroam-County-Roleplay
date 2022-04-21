/// <reference types='@altv/types-client' />
/// <reference types='@altv/types-natives' />
import * as alt from 'alt-client';
import * as native from 'natives';

const player = alt.Player.local;

let bodyCam = null;
let bodyCamStart = null;

alt.onServer('face:createCamera', createCamera);
alt.on('face:createCamera', createCamera);
function createCamera(){
    bodyCamStart = player.pos;
    let camValues = { Angle: native.getEntityRotation(player, 2).z + 90, Dist: 2.6, Height: 0.2};
    let pos = getCameraOffset(new alt.Vector3(bodyCamStart.x, bodyCamStart.y, bodyCamStart.z + camValues.Height), camValues.Angle, camValues.Dist);
    bodyCam = native.createCamWithParams('DEFAULT_SCRIPTED_CAMERA', pos.x, pos.y, pos.z, 0, 0, 0, 50, true, 2);
    native.pointCamAtCoord(bodyCamStart.x, bodyCamStart.y, bodyCamStart.z + camValues.Height);
    native.setCamActive(bodyCam);
    native.renderScriptCams(bodyCam);
    alt.emit('animation:play', 'amb@world_human_guard_patrol@male@base', 'base', -1, 49, false);
}

alt.onServer('face:setCamera', setCamera);
alt.on('face:setCamera', setCamera);
function setCamera(id){
    let camValues = { Angle: 0, Dist: 1, Height: 0.2 };
    switch(id){
        case 0:
            //torso
            camValues = { Angle: 0, Dist: 2.6, Height: 0.2 };
            break;
        case 1:
            //head
            camValues = { Angle: 0, Dist: 1, Height: 0.5 };
            break;
        case 2:
            //hair / bear / eyebrows
            camValues = { Angle: 0, Dist: 0.5, Height: 0.7 };
			break;
        case 3:
            //chesthair
            camValues = { Angle: 0, Dist: 1, Height: 0.2 };
			break;
    }

    const camPos = getCameraOffset(new alt.Vector3(bodyCamStart.x, bodyCamStart.y, bodyCamStart.z + camValues.Height), native.getEntityRotation(player, 2).z + 90 + camValues.Angle, camValues.Dist);
    native.setCamCoord(bodyCam, camPos.x, camPos.y, camPos.z);
    native.pointCamAtCoord(bodyCamStart.x, bodyCamStart.y, bodyCamStart.z + camValues.Height);
}

alt.onServer('face:destroyCamera', destroyCamera);
alt.on('face:destroyCamera', destroyCamera);
function destroyCamera(){
    if(bodyCam == null) return;
    native.setCamActive(bodyCam, false);
    native.destroyCam(bodyCam);
    native.renderScriptCams(false, false, 3000, true, true, 2);
    bodyCam = null;
    alt.emit('animation:stop');
}

function getCameraOffset(pos, angle, dist) {
    angle = angle * 0.0174533;
    pos.y = pos.y + dist * Math.sin(angle);
    pos.x = pos.x + dist * Math.cos(angle);
    return pos;
}


let faceView = null;

//Face UI
alt.onServer('face:load', (data) => {
    if(faceView == null){
        faceView = new alt.WebView('http://resource/webview/character/face/person1.html');
        faceView.isVisible = true;
        faceView.focus();
        alt.showCursor(true);

        faceView.on('face:client-backCreate', () => {
            alt.emitServer('ClientCharCreationBack');
        });
        
        faceView.on('face:client-nextCreate', (forename, surname, age) => {
            alt.emit('Display_Creator_part2');
        });

        faceView.on('face:client-cameraTo', (type) => {
            alt.emit('face:setCamera', type);
        });

        faceView.on('face:client-onChange', (id, value) => {
            alt.emitServer('ClientOnRangeChange', id, value);
        });

        alt.showCursor(true);
    }
});

alt.onServer('face:destroy', () => {
    if(faceView != null){
        faceView.isVisible = false;
        faceView.unfocus();
        faceView.destroy();
        faceView = null;
        alt.showCursor(false);
    }
})

alt.onServer('face:load2', (data) => {
    if(faceView == null){
        faceView = new alt.WebView('http://resource/webview/character/face/person2.html');
        faceView.isVisible = true;
        faceView.focus();
        alt.showCursor(true);
    }
});