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
    let pos = getCameraOffset(bodyCamStart.x, bodyCamStart.y, bodyCamStart.z + camValues.Height, camValues.Angle, camValues.Dist);
    bodyCam = native.createCamWithParams('DEFAULT_SCRIPTED_CAMERA', pos.x, pos.y, pos.z, 0, 0, 0, 50, true, 2);
    native.pointCamAtCoord(bodyCam, bodyCamStart.x, bodyCamStart.y, bodyCamStart.z + camValues.Height);
    native.setCamActive(bodyCam, true);
    native.renderScriptCams(true, false, 0, true, true, 1);
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

    const camPos = getCameraOffset(bodyCamStart.x, bodyCamStart.y, bodyCamStart.z + camValues.Height, native.getEntityRotation(player, 2).z + 90 + camValues.Angle, camValues.Dist);
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

function getCameraOffset(posX, posY, posZ, angle, dist) {
    angle = angle * 0.0174533;
    posY = posY + dist * Math.sin(angle);
    posX = posX + dist * Math.cos(angle);
    return { x: posX, y: posY, z: posZ };
}


let faceView = null;

//Face UI
alt.onServer('face:load', (data) => {
    if(faceView == null){
        faceView = new alt.WebView('http://resource/webview/character/face/person1.html');
        faceView.isVisible = true;
        faceView.focus();
        faceView.emit('face:view-loadNewCharacter', data);
        alt.showCursor(true);

        faceView.on('face:client-backCreate', () => {
            alt.emitServer('ClientCharCreationBack');
        });
        
        faceView.on('face:client-nextCreate', (forename, surname, age) => {
            alt.emitServer('Display_Creator_part2', forename, surname, age);
        });

        faceView.on('face:client-cameraTo', (type) => {
            alt.emit('face:setCamera', type);
        });

        faceView.on('face:client-onChange', (id, value) => {
            alt.emitServer('ClientOnRangeChange', String(id), String(value));
        });

        alt.showCursor(true);
    }
});

alt.onServer('face:destroy', destroyFace);
alt.on('face:destroy', destroyFace);
function destroyFace(){
    if(faceView != null){
        faceView.isVisible = false;
        faceView.unfocus();
        faceView.destroy();
        faceView = null;
        alt.showCursor(false);
    }
}

alt.onServer('face:load2', (data) => {
    if(faceView == null){
        faceView = new alt.WebView('http://resource/webview/character/face/person2.html');
        faceView.isVisible = true;
        faceView.focus();
        faceView.emit('face2:view-loadFeature', data);
        alt.showCursor(true);

        faceView.on('face2:client-backCreate', () => {
            alt.emit('face:destroy');
            alt.emitServer('ShowPlayerCreator');
        });

        faceView.on('face2:client-nextCreate', () => {
            alt.emitServer('Display_Creator_part3');
        });

        faceView.on('face2:client-cameraTo', (type) => {
            alt.emit('face:setCamera', type);
        });

        faceView.on('face2:client-onChange', (id, value) => {
            alt.emitServer('ClientOnRangeChange', String(id), String(value));
        });

        faceView.on('face2:client-setFaceFeature', (id, value) => {
            alt.emitServer('ClientSetFaceFeature', String(id), String(value));
        });
    }
});

alt.onServer('face:load3', (data) => {
    if(faceView == null){
        faceView = new alt.WebView('http://resource/webview/character/face/person3.html');
        faceView.isVisible = true;
        faceView.focus();
        faceView.emit('face3:view-loadClothing', data);
        alt.showCursor(true);
        
        faceView.on('face3:client-backCreate', () => {
            alt.emitServer('ClientCharCreation3Back');
        });

        faceView.on('face3:client-nextCreate', () => {
            alt.emitServer('ClientCharCreation3Next');
        });

        faceView.on('face3:client-cameraTo', (type) => {
            alt.emit('face:setCamera', type);
        });

        faceView.on('face3:client-onChange', (id, value) => {
            alt.emitServer('ClientOnRangeChange', String(id), String(value));
        });

        faceView.on('face3:client-setClothes', (type, value) => {

        });
    }
});