/// <reference types="@altv/types-client" />
/// <reference types="@altv/types-natives" />
import * as alt from 'alt-client';
import * as native from 'natives';

import "./face";

let characterView = null;

alt.onServer('character:load', (data) => {
    if(characterView == null){
        characterView = new alt.WebView('http://resource/webview/character/index.html');
        characterView.emit('character:view-loadCharacters', data);
        characterView.isVisible = true;
        characterView.focus();
        alt.showCursor(true);
    
        alt.emit("camera:create", -533.1306, -222.414, 38.14975, -10, 0, 2, 60);

        characterView.on('character:client-previewCharacter', (id) => {

        });

        characterView.on('character:client-selectCharacter', (id) => {

        });

        characterView.on('character:client-createCharacter', () => {
            alt.emitServer("RequestCreateCharacter");
        });
    }
});

alt.onServer('character:destroy', () => {
    if(characterView != null){
        characterView.isVisible = false;
        characterView.unfocus();
        characterView.destroy();
        characterView = null;
        alt.showCursor(false);
        alt.emit("camera:destroy");
    }
})