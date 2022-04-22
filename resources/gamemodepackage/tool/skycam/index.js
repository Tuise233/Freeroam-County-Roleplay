/// <reference types='@altv/types-client' />
import * as alt from 'alt';
import * as native from 'natives';

const localPlayer = alt.Player.local;


alt.onServer('skycam:toggle', (active) => {
    if(active){
        native.switchOutPlayer(localPlayer, 0, 1);
    } else {
        native.switchInPlayer(localPlayer);
        setTimeout(() => {
            alt.emitServer('skycam:end');
        }, 4000);
    }
})