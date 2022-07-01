/// <reference types="@altv/types-client" />
/// <reference types="@altv/types-natives" />
import * as alt from 'alt-client';
import * as native from 'natives';

let characterView = null;

alt.onServer("character:load", () => {
    if(characterView == null){
        characterView = new alt.WebView("http://resource/webview/character/index.html");
        characterView.isVisible = true;
        characterView.focus();
        alt.showCursor(true);

        characterView.on("character:client-toggleSex", (sex) => {
            alt.emitServer("ToggleSex", sex);
        });

        characterView.on("character:client-randomModel", (sex, model) => {
            alt.emitServer("RandomModel", sex, model);
        });

        characterView.on("character:client-createCharacter", (username, age, sex, model) => {
            alt.emitServer("CreateCharacter", username, age, sex, model);
        });
    }
});

alt.onServer("character:destroy", () => {
    if(characterView != null){
        characterView.unfocus();
        characterView.destroy();
        characterView = null;
        alt.showCursor(false);
    }
})