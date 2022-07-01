const app = new Vue({
    el: "#app",
    data: {
        name: "",
        age: 18,
        sex: 1,
        model: 0,
        maleModel: 5,
        femaleModel: 3
    },
    methods: {
        toggleSex(sex){
            this.sex = sex;
            alt.emit("character:client-toggleSex", this.sex);
        },

        randomModel(){
            this.model = this.sex == 1 ? this.random(0, this.maleModel) : this.random(0, this.femaleModel);
            alt.emit("character:client-randomModel", this.sex, this.model);
        },

        createCharacter(){
            alt.emit("character:client-createCharacter", this.name, this.age, this.sex, this.model);
        },

        random(start, end) {
            return Math.floor(Math.random() * (end - start) + start)
        }
    },
});