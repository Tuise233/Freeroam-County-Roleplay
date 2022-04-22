/// <reference types="@altv/types-client" />
/// <reference types="@altv/types-natives" />
import * as alt from 'alt-client';
import * as native from 'natives';

let camera = null;

alt.on("camera:create", createCamera);
alt.on("camera:destroy", destroyCamera);

function createCamera(x, y, z, rx, ry, rz, fov){
    if(camera == null){
        camera = native.createCamWithParams("DEFAULT_SCRIPTED_CAMERA", x, y, z, rx, ry, rz, fov, true, 2);
        native.renderScriptCams(true, false, 0, true, true, 1);
    }
}

function destroyCamera(){
    if(camera != null){
        native.destroyCam(camera, true);
        camera = null;
    }
}