/// <reference types="@altv/types-client" />
/// <reference types="@altv/types-natives" />
import * as alt from 'alt-client';
import * as native from 'natives';

const localPlayer = alt.Player.local;

let freeze = false;

alt.onServer('freeze:toggle', freezeToggle);
alt.on("freeze:toggle", freezeToggle);
function freezeToggle(state, position){
    freeze = state;
    native.freezeEntityPosition(localPlayer, position);
}

alt.onServer("cursor:show", showCursor);
function showCursor(state){
    alt.showCursor(state);
}

alt.everyTick(() => {
    if(freeze){
        native.disableAllControlActions(2);

        native.enableControlAction(2, 1, true);
        native.enableControlAction(2, 2, true);
        native.enableControlAction(2, 3, true);
        native.enableControlAction(2, 4, true);
        native.enableControlAction(2, 5, true);
        native.enableControlAction(2, 6, true);
        native.enableControlAction(2, 249, true);
        native.enableControlAction(2, 286, true);
        native.enableControlAction(2, 287, true);
        native.enableControlAction(2, 290, true);
        native.enableControlAction(2, 291, true);
        native.enableControlAction(2, 292, true);
        native.enableControlAction(2, 293, true);
        native.enableControlAction(2, 294, true);
        native.enableControlAction(2, 295, true);
        native.enableControlAction(2, 270, true);
        native.enableControlAction(2, 271, true);
        native.enableControlAction(2, 272, true);
        native.enableControlAction(2, 273, true);
        native.enableControlAction(2, 329, true);
        native.enableControlAction(2, 330, true);

		native.disableControlAction(2, 71, true);
		native.disableControlAction(2, 72, true);
    }
})