/// <reference types="@altv/types-client" />
/// <reference types="@altv/types-natives" />
import * as alt from 'alt-client';
import * as native from 'natives';

let chatView = null;

alt.onServer("chat:load", () => {
    if (chatView == null) {
        chatView = new alt.WebView("http://resource/webview/chat/index.html");
        chatView.isVisible = false;

        chatView.on("chat:client-pushMessage", (message) => {
            if (message != "") {
                //解析是否是指令
                if (message.includes("/") && message[0] == "/") {
                    let split = message.split(" ");
                    let command = split[0].replace("/", "");
                    let params = split.splice(1, split.length);
                    alt.emitServer("PushCommand", command, params);
                } else {
                    alt.emitServer("PushMessage", message);
                }
            }
            alt.emit("freeze:toggle", false, false);
            chatView.unfocus();
        });
    }
});

alt.on("chat:showInputBox", () => {
    if (chatView != null && chatView.isVisible == true) {
        chatView.emit("chat:view-showInputBox");
        chatView.focus();
        alt.emit("freeze:toggle", true, false);
    }
});

alt.onServer("chat:toggleChatBox", (state) => {
    if (chatView != null) {
        chatView.isVisible = state;
        chatView.emit("chat:view-toggleChatBox", state);
    }
});

alt.onServer("chat:pushMessage", (message) => {
    if (chatView != null) {
        chatView.emit("chat:view-pushMessage", message);
    }
});