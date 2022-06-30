/// <reference types="@altv/types-client" />
/// <reference types="@altv/types-natives" />
import * as alt from 'alt-client';
import * as native from 'natives';

let accountView = null;

alt.onServer("account:load", () => {
    if(accountView == null){
        alt.showCursor(true);
        accountView = new alt.WebView("http://resource/webview/account/index.html");
        accountView.isVisible = true;
        accountView.focus();

        accountView.on("account:client-Login", (name, password) => {
            if(!name || !password){
                return displayErr("您的用户名或密码未填写");
            }

            if(name.length < 5 || name.length > 18){
                return displayErr("用户名长度规定在5~18个单位");
            }

            if(password.length < 5 || password.length > 18){
                return displayErr("密码长度规定在5~18个单位");
            }

            alt.emitServer("LoginAccount", name, password);
        });

        accountView.on("account:client-Register", (name, password, email) => {
            if(!name || !password){
                return displayErr("您的用户名或密码未填写");
            }

            if(name.length < 5 || name.length > 18){
                return displayErr("用户名长度规定在5~18个单位");
            }

            if(password.length < 5 || password.length > 18){
                return displayErr("密码长度规定在5~18个单位");
            }

            if(!email || !email.includes("@")){
                return displayErr("您输入的邮箱无效");
            }

            alt.emitServer("RegisterAccount", name, password, email);
        });
    }
});

alt.onServer("account:destroy", () => {
    if(accountView != null){
        accountView.unfocus();
        accountView.destroy();
        accountView = null;
        alt.showCursor(false);
    }
});

alt.onServer("account:client-displayErr", displayErr);
function displayErr(type){
    accountView.emit("account:view-displayErrorMsg", type);
}