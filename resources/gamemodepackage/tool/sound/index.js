/// <reference types="@altv/types-client" />
/// <reference types="@altv/types-natives" />
import * as alt from 'alt-client';
import * as native from 'natives';

import { soundList } from './soundList';

alt.on("sound:play", playSound);
alt.onServer("sound:play", playSound);
function playSound(index){
    if(!isNaN(index)){
        for(let i = 0; i < soundList.length; i++){
            if(soundList[i].id == Number(index)){
                let soundName = soundList[Number(index)].soundName;
                let soundSetName = soundList[Number(index)].soundSetName;
                native.playSoundFrontend(index, soundName, soundSetName, false);
            }
        }
    }
}