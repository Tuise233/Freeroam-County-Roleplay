/// <reference types="@altv/types-client" />
import * as alt from 'alt-client';
import * as native from 'natives';

const localPlayer = alt.Player.local;

let syncPlayer = [];
let interval = null;



// alt.everyTick(()=>{
//     if(native.isPedRunningMeleeTask(alt.Player.local)){
//         //近战中
//     }else
//     {
//         if(native.isPlayerFreeForAmbientTask(alt.Player.local))return;
//         DisableControl();
//     }
// });

function DisableControl() {
    native.disableControlAction(0, 12, true);//Weapon
    native.disableControlAction(0, 13, true);//Weapon
    native.disableControlAction(0, 14, true);//Weapon
    native.disableControlAction(0, 15, true);//Weapon
    native.disableControlAction(0, 16, true);//Weapon
    native.disableControlAction(0, 17, true);//Weapon

    native.disableControlAction(0, 24, true);//Left Mouse
    native.disableControlAction(0, 25, true);//Right Mouse
    native.disableControlAction(0, 37, true);//TAB

    native.disableControlAction(0, 157, true);//Weapon Taste: 1
    native.disableControlAction(0, 158, true);//Weapon Taste: 2
    native.disableControlAction(0, 160, true);//Weapon Taste: 3
    native.disableControlAction(0, 164, true);//Weapon Taste: 4
    native.disableControlAction(0, 165, true);//Weapon Taste: 5
    native.disableControlAction(0, 159, true);//Weapon Taste: 6
    native.disableControlAction(0, 161, true);//Weapon Taste: 7
    native.disableControlAction(0, 162, true);//Weapon Taste: 8
    native.disableControlAction(0, 163, true);//Weapon Taste: 9

    native.disablePlayerFiring(alt.Player.local.scriptID, false);//Weapon Fire
}

alt.onServer("animation:init", () => {
    alt.Player.local.setMeta('isAnyAnim', false);
    alt.Player.local.setMeta('specialAnim', false);
})

alt.onServer("animation:toggle", (state) => {
    alt.Player.local.setMeta('isAnyAnim', state);
})

/*
    animation:play
    执行播放动作Task

    Args:
    animDict: string
    animName: string
    duration: int,
    flag: int,
    lockpos: bool
*/
alt.onServer('animation:play', playAnimation);
alt.on('animation:play', playAnimation);
export function playAnimation(animDict, animName, duration, flag, lockpos){
    //alt.emitServer("SetSyncedMetaData","animation", `${animDict}|${animName}|${duration}|${flag}|${lockpos}`);
    localPlayer.setMeta('isAnyAnim', true);
    native.requestAnimDict(animDict);
    if(interval){
        clearInterval(interval);
        interval = null;
    }
    interval = setInterval(() => {
        if(native.hasAnimDictLoaded(animDict)){
            if(interval){
                clearInterval(interval);
                interval = null;
            }
            native.taskPlayAnim(alt.Player.local, animDict, animName, 8.0, 1, duration, flag, 1, lockpos, lockpos, lockpos);
            setTimeout(() => {
                stopAnimation();
            }, duration);
        }
    }, 100);
}


/*
    animation:stop
    停止播放动作Task
*/
alt.onServer('animation:stop', stopAnimation);
alt.on('animation:stop', stopAnimation);
export function stopAnimation () {
    localPlayer.setMeta('isAnyAnim', false);
    localPlayer.setMeta('specialAnim', false);
    //alt.emitServer("DeleteSyncedMetaData","animation");
    native.clearPedTasks(native.playerPedId());
}