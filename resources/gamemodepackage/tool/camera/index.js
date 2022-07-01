/// <reference types="@altv/types-client" />
import * as alt from 'alt-client';
import * as native from 'natives';

let cameraControlsInterval;
let camera;
let zpos = 0.2;
let fov = 90;
let startPosition;
let startCamPosition;
let timeBetweenAnimChecks = Date.now() + 100;

const localPlayer = alt.Player.local;

//alt Event
alt.onServer("camera:start", createPedEditCamera);
alt.onServer("camera:stop", destroyPedEditCamera);
alt.on("camera:start", createPedEditCamera);
alt.on("camera:stop", destroyPedEditCamera);

var IsCamareRending = false;
export function createPedEditCamera(cx = null, cy = null, cz = null, fx = null, fy = null, fz = null) {
    if(IsCamareRending){
        return;
    }
    IsCamareRending = true;
    startPosition = { ...localPlayer.pos };
    if (!camera) {
        fov = 60;
        zpos = 0.2;

        const pedrotation = native.getEntityRotation(localPlayer,0);
        const forwardVector = native.getEntityForwardVector(localPlayer.scriptID);
        let forwardCameraPosition = {};

        if(cx == null){
            forwardCameraPosition = {
                x: startPosition.x + forwardVector.x * 1.2,
                y: startPosition.y + forwardVector.y * 1.2,
                z: startPosition.z + zpos
            };
        } else {
            forwardCameraPosition = {
                x: cx,
                y: cy,
                z: cz + zpos
            };
        }

        startCamPosition = forwardCameraPosition;

        camera = native.createCamWithParams(
            'DEFAULT_SCRIPTED_CAMERA',
            forwardCameraPosition.x,
            forwardCameraPosition.y,
            forwardCameraPosition.z,
            0,
            0,
            pedrotation.z,
            fov,
            true,
            0
        );
        if(fx == null){
            native.pointCamAtCoord(camera, startPosition.x, startPosition.y, startPosition.z);
        } else {
            native.pointCamAtCoord(camera, fx, fy, fz);
        }
        native.setCamActive(camera, true);
        native.renderScriptCams(true, false, 0, true, false, 0);
    }

    if (cameraControlsInterval !== undefined || cameraControlsInterval !== null) {
        if(cameraControlsInterval)
            alt.clearInterval(cameraControlsInterval);
        cameraControlsInterval = null;
    }
    cameraControlsInterval = alt.setInterval(handleControls, 0);
}

export function destroyPedEditCamera() {
    if(!IsCamareRending){
        return;
    }
    IsCamareRending = false;

    if (cameraControlsInterval !== undefined || cameraControlsInterval !== null) {
        if(cameraControlsInterval)
            alt.clearInterval(cameraControlsInterval);
        cameraControlsInterval = null;
    }

    if (camera) {
        camera = null;
    }

    native.destroyAllCams(true);
    native.renderScriptCams(false, false, 0, false, false, 0);

    zpos = 0.2;
    fov = 60;
    startPosition = null;
    startCamPosition = null;
}

function handleControls() {
    // native.hideHudAndRadarThisFrame();
    native.disableAllControlActions(0);
    native.disableAllControlActions(1);
    native.disableControlAction(0, 0, true);
    native.disableControlAction(0, 1, true);
    native.disableControlAction(0, 2, true);
    native.disableControlAction(0, 24, true);
    native.disableControlAction(0, 25, true);
    native.disableControlAction(0, 32, true); // w
    native.disableControlAction(0, 33, true); // s
    native.disableControlAction(0, 34, true); // a
    native.disableControlAction(0, 35, true); // d

    const [_, width] = native.getActiveScreenResolution(0, 0);
    const cursor = alt.getCursorPos();
    const _x = cursor.x;
    let oldHeading = native.getEntityHeading(localPlayer.scriptID);

    // Scroll Up
    if (native.isDisabledControlPressed(0, 15)) {
        if (_x < width / 2 + 250 && _x > width / 2 - 250) {
            fov -= 2;

            if (fov < 10) {
                fov = 10;
            }

            native.setCamFov(camera, fov);
            native.setCamActive(camera, true);
            native.renderScriptCams(true, false, 0, true, false, 0);
            // alt.log(`fov: ${fov}`);
        }
    }

    // SCroll Down
    if (native.isDisabledControlPressed(0, 16)) {
        if (_x < width / 2 + 250 && _x > width / 2 - 250) {
            fov += 2;

            if (fov > 130) {
                fov = 130;
            }

            native.setCamFov(camera, fov);
            native.setCamActive(camera, true);
            native.renderScriptCams(true, false, 0, true, false, 0);
            // alt.log(`fov: ${fov}`);
        }
    }

    if (native.isDisabledControlPressed(0, 32)) {
        zpos += 0.01;

        if (zpos > 1.2) {
            zpos = 1.2;
        }

        native.setCamCoord(camera, startCamPosition.x, startCamPosition.y, startCamPosition.z + zpos);
        native.pointCamAtCoord(camera, startPosition.x, startPosition.y, startPosition.z + zpos);
        native.setCamActive(camera, true);
        native.renderScriptCams(true, false, 0, true, false, 0);
        // alt.log(`zpos: ${zpos}`);
    }

    if (native.isDisabledControlPressed(0, 33)) {
        zpos -= 0.01;

        if (zpos < -1.2) {
            zpos = -1.2;
        }

        native.setCamCoord(camera, startCamPosition.x, startCamPosition.y, startCamPosition.z + zpos);
        native.pointCamAtCoord(camera, startPosition.x, startPosition.y, startPosition.z + zpos);
        native.setCamActive(camera, true);
        native.renderScriptCams(true, false, 0, true, false, 0);
        // alt.log(`zpos: ${zpos}`);
    }

    // rmb
    if (native.isDisabledControlPressed(0, 25)) {
        // Rotate Negative
        if (_x < width / 2) {
            const newHeading = (oldHeading -= 2);
            if(localPlayer.vehicle != null){
                native.setEntityHeading(localPlayer.vehicle.scriptID, newHeading);
            } else {
                native.setEntityHeading(localPlayer.scriptID, newHeading);
            }
            // alt.log(`newHeading: ${newHeading}`);
        }

        // Rotate Positive
        if (_x > width / 2) {
            const newHeading = (oldHeading += 2);
            if(localPlayer.vehicle != null){
                native.setEntityHeading(localPlayer.vehicle.scriptID, newHeading);
            } else {
                native.setEntityHeading(localPlayer.scriptID, newHeading);
            }
            // alt.log(`newHeading: ${newHeading}`);
        }
    }

    // d
    if (native.isDisabledControlPressed(0, 35)) {
        const newHeading = (oldHeading += 2);
        if(localPlayer.vehicle != null){
            native.setEntityHeading(localPlayer.vehicle.scriptID, newHeading);
        } else {
            native.setEntityHeading(localPlayer.scriptID, newHeading);
        }
        // alt.log(`newHeading: ${newHeading}`);
    }

    // a
    if (native.isDisabledControlPressed(0, 34)) {
        const newHeading = (oldHeading -= 2);
        if(localPlayer.vehicle != null){
            native.setEntityHeading(localPlayer.vehicle.scriptID, newHeading);
        } else {
            native.setEntityHeading(localPlayer.scriptID, newHeading);
        }
        // alt.log(`newHeading: ${newHeading}`);
    }

    if (Date.now() > timeBetweenAnimChecks) {
        timeBetweenAnimChecks = Date.now() + 1500;
        if (!native.isEntityPlayingAnim(localPlayer.scriptID, 'nm@hands', 'hands_up', 3)) {
            alt.emit('animation:Play', {
                dict: 'nm@hands',
                name: 'hands_up',
                duration: -1,
                flag: 2
            });
        }
    }
}

alt.onServer("camera:setFov", setFov);
alt.on("camera:setFov", setFov);
export function setFov(value) {
    fov = value;

    native.setCamFov(camera, fov);
    native.setCamActive(camera, true);
    native.renderScriptCams(true, false, 0, true, false, 0);
}

alt.onServer("camera:setZPos", setZPos);
alt.on("camera:setZPos", setZPos);
export function setZPos(value) {
    zpos = value;

    native.setCamCoord(camera, startCamPosition.x, startCamPosition.y, startCamPosition.z + zpos);
    native.pointCamAtCoord(camera, startPosition.x, startPosition.y, startPosition.z + zpos);
    native.setCamActive(camera, true);
    native.renderScriptCams(true, false, 0, true, false, 0);
}

alt.on("camera:setHeading", setHeading);
export function setHeading(value){
    native.setEntityHeading(localPlayer.scriptID, value);
}