/// <reference types="@altv/types-client" />
/// <reference types="@altv/types-natives" />
import * as alt from 'alt-client';
import * as native from 'natives';

alt.on("keydown", (key) => {
    switch(key){
        case 84:{
            alt.emit("chat:showInputBox");
            break;
        }
    }
});