/// <reference types="@altv/types-client" />
/// <reference types="@altv/types-natives" />
import * as alt from 'alt-client';
import * as native from 'natives';

let accountView = null;

alt.onServer('account:load', () => {
    if(accountView == null){
        alt.showCursor(true);
        accountView = new alt.WebView('http://resource/webview/account/index.html');
        accountView.isVisible = false;
        accountView.focus();

        accountView.on('accont:client-Login', (name, password) => {
            console.log(name, password);

            if(!name || !password){
                return displayErr('您的用户名或密码未填写');
            }

            if(name.length < 5 || name.length > 15){
                return displayErr('您的用户名长度不在5~15个单位内');
            }

            if(password.length < 5 || password.length > 15){
                return displayErr('您的密码长度不在5~15个单位内');
            }

            alt.emitServer('LoginAccount', name, password);
        });

        accountView.on('account:client-Register', (name, password, email) => {
            console.log(name, password, email);

            if(!name || !password){
                return displayErr('您的用户名或密码未填写');
            }

            if(name.length < 5 || name.length > 15){
                return displayErr('您的用户名长度不在5~15个单位内');
            }

            if(password.length < 5 || password.length > 15){
                return displayErr('您的密码长度不在5~15个单位内');
            }

            if(!email || email.indexOf('@') === -1){
                return displayErr('您的邮箱无效');
            }

            alt.emitServer('RegisterAccount', name, password, email);
        });
    }
});


alt.onServer('account:client-displayErr', displayErr);
function displayErr(type){
    accountView.emit('account:view-displayErrorMsg', type);
}


alt.onServer('account:destroy', () => {
    if(accountView != null){
        accountView.unfocus();
        accountView.destroy();
        alt.showCursor(false);
    }
});