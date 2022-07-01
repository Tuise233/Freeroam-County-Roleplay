const app = new Vue({
    el: "#app",
    data: {
        //最多存放15条消息
        prevMessage: [],
        prevIndex: 0,
        history: [],
        inputText: "",
        isShowInput: false,
        isShowChat: false
    },
    created() {
        setTimeout(() => {
            this.isShowInput = false;
        }, 50);
    },
    methods: {
        onKeyPressEnter() {
            if (this.isShowInput) {
                this.isShowInput = false;
                alt.emit("chat:client-pushMessage", this.inputText);
                if (this.inputText != "") {
                    if (this.prevMessage.length >= 10) {
                        this.prevMessage.splice(this.prevMessage.length - 1, this.prevMessage.length);
                    }
                    this.inputText = "";
                }
                if(this.prevMessage.length > 0){
                    this.prevIndex = this.prevMessage.length - 1;
                }
            }
        },

        onKeyPressArrowUp() {
            if (this.isShowInput) {
                if(this.prevMessage.length > 0){
                    this.inputText = this.prevMessage[this.prevIndex];
                    setTimeout(() => {
                        this.$refs.inputBox.selectionStart = this.$refs.inputBox.selectionEnd = this.$refs.inputBox.value.length;
                    }, 0);
                    if(this.prevIndex > 0){
                        this.prevIndex -= 1;
                    }
                }
            }
        },

        onKeyPressArrowDown(){
            if(this.isShowInput){
                if(this.prevMessage.length > 0){
                    if(this.prevIndex == this.prevMessage.length - 1){
                        this.inputText = "";
                    } else {
                        this.prevIndex += 1;
                        this.inputText = this.prevMessage[this.prevIndex];
                        setTimeout(() => {
                            this.$refs.inputBox.selectionStart = this.$refs.inputBox.selectionEnd = this.$refs.inputBox.value.length;
                        }, 0);
                    }
                }
            }
        },

        pushMessage(message) {
            if(this.history.length >= 15){
                this.history.splice(0, 1);
            }
            let date = new Date();
            let hour = date.getHours().toString().length == 1 ? `0${date.getHours()}` : date.getHours();
            let minute = date.getMinutes().toString().length == 1 ? `0${date.getMinutes()}` : date.getMinutes();
            let second = date.getSeconds().toString().length == 1 ? `0${date.getSeconds()}` : date.getSeconds();
            this.history.push(`<font color='#3CB371'>[${hour}:${minute}:${second}]</font> ${message}`);
            this.$nextTick(() => {
                this.$refs.chatBox.scrollTop = this.$refs.chatBox.scrollHeight;
            });
        },

        toggleChatBox(state){
            this.isShowChat = state;
        },

        showInputBox(){
            if(!this.isShowInput){
                this.isShowInput = true;
                this.$nextTick(() => {
                    this.$refs.inputBox.focus();
                    setTimeout(() => {
                        this.inputText = "";
                    }, 0);
                })
            }
        }
    }
});

alt.on("chat:view-showInputBox", app.showInputBox);
alt.on("chat:view-toggleChatBox", app.toggleChatBox);
alt.on("chat:view-pushMessage", app.pushMessage);

document.onkeydown = (e) => {
    switch (e.keyCode) {
        case 13: {
            app.onKeyPressEnter();
            break;
        }

        case 38: {
            app.onKeyPressArrowUp();
            break;
        }

        case 40:{
            app.onKeyPressArrowDown();
            break;
        }
    }
}