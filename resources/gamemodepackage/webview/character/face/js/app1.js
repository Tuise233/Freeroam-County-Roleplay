$('.select-gender .gender').click(function (val, data) {
    $('.gender.active').removeClass('active');
    $(this).addClass('active');
    $(this).parents('.select-gender').find('input').attr('value', $(this).attr('data-value'));
    var genderVal = document.getElementById('gender-value').value
    OnRangeChange("range_gender", genderVal, data)
});
$('.camera-buttons .button').click(function() {
    $('.camera-buttons .button.active').removeClass('active');
    $(this).addClass('active');
})
$(document).ready(function () {
    $('.ui.form').form({
        fields: {
            firstname: {
                identifier: 'first-name',
                rules: [{
                    type: 'empty',
                    prompt: '请输入姓氏'
                }]
            },
            lastname: {
                identifier: 'last-name',
                rules: [{
                    type: 'empty',
                    prompt: '请输入名字'
                }]
            },
            age: {
                identifier: 'age',
                rules: [{
                    type: 'empty',
                    prompt: '请输入年龄'
                }]
            },
            sex: {
                identifier: 'gender',
                rules: [{
                    type: 'minCount[1]',
                    prompt: '请选择性别'
                }]
            }
            
        }
    });

    $("#charcreation_back").click(function () {
        alt.emit('face:client-backCreate');
    });

    $("#charcreation_next").click(function () {
        if($("#creation_age").val() == null)
        {
            $("#creation_age").val() = 18;
        }
        if($("#creation_forname").val() != "" && $("#creation_surname").val() != ""){
            //mp.trigger("ClientCharCreationNext", $("#creation_forename").val(), $("#creation_surname").val(), $("#creation_age").val());
            alt.emit('face:client-nextCreate', $("#creation_forename").val(), $("#creation_surname").val(), $("#creation_age").val());
        }
    });

    $("#charcreation_head").click(function () {
        //mp.trigger("cameraPointTo", 1);
        alt.emit('face:client-cameraTo', 1);
    });

    $("#charcreation_body").click(function () {
        //mp.trigger("cameraPointTo", 0);
        alt.emit('face:client-cameraTo', 0);
    });
});

// parseFloat(val.toFixed(1))
function LoadNewCharacter(arr_data) {

    let data = JSON.parse(arr_data);
    $("#creation_forename").val(data[0].Forename);
    $("#creation_surname").val(data[0].Surname);
    $('#gender-value').val(data[0].Gender);

    $('#range_base').range({
        min: 0,
        max: 22,
        start: data[0].Base,
        step: 1,
        smooth: false,
        onChange: function (val, data) {
            OnRangeChange("range_base", val, data), $('#display-base').html(val)
        }
    });
    $('#range_base2').range({
        min: 0,
        max: 24,
        start: data[0].Base2,
        step: 1,
        smooth: false,
        onChange: function (val, data) {
            OnRangeChange("range_base2", val, data), $('#display-base2').html(val)
        }
    });
    $('#range_baseblend').range({
        min: 0,
        max: 9,
        start: data[0].BaseBlend,
        step: 1,
        smooth: false,
        onChange: function (val, data) {
            OnRangeChange("range_baseblend", val, data)
        }
    });
    $('#range_skin').range({
        min: 0,
        max: 9,
        start: data[0].Skin,
        step: 1,
        smooth: false,
        onChange: function (val, data) {
            OnRangeChange("range_skin", val, data), $('#display-skin').html(val)
        }
    });
    $('#range_eyes').range({
        min: 0,
        max: 6,
        start: data[0].Eyes,
        step: 1,
        smooth: false,
        onChange: function (val, data) {
            OnRangeChange("range_eyes", val, data), $('#display-eyes').html(val)
        }
    });
    $('#range_hair').range({
        min: 0,
        max: 72,
        start: data[0].Hair,
        step: 1,
        smooth: false,
        onChange: function (val, data) {
            OnRangeChange("range_hair", val, data), $('#display-hair').html(val)
        }
    });
    $('#range_haircolor').range({
        min: 0,
        max: 63,
        start: data[0].HairColor,
        step: 1,
        smooth: false,
        onChange: function (val, data) {
            OnRangeChange("range_haircolor", val, data), $('#display-haircolor').html(val)
        }
    });
    $('#range_haircolor2').range({
        min: 0,
        max: 63,
        start: data[0].HairHighlightColor,
        step: 1,
        smooth: false,
        onChange: function (val, data) {
            OnRangeChange("range_haircolor2", val, data), $('#display-haircolor2').html(val)
        }
    });
    $('#range_eyebrows').range({
        min: 0,
        max: 31,
        start: data[0].Eyebrows,
        step: 1,
        smooth: false,
        onChange: function (val, data) {
            OnRangeChange("range_eyebrows", val, data), $('#display-eyebrows').html(val)
        }
    });
    $('#range_beard').range({
        min: 0,
        max: 28,
        start: data[0].Beard,
        step: 1,
        smooth: false,
        onChange: function (val, data) {
            OnRangeChange("range_beard", val, data), $('#display-beard').html(val)
        }
    });
    $('#range_rotation').range({
        min: 0,
        max: 360,
        start: 100,
        step: 20,
        smooth: false,
        onChange: function (val, data) {
            OnRangeChange("range_rotation", val, data)
        }
    });
    $('#range_elevation').range({
        min: -2,
        max: 1,
        start: 0,
        step: 1,
        smooth: false,
        onChange: function (val, data) {
            OnRangeChange("range_elevation", val, null)
        }
    });
}

function OnRangeChange(id, val, data) {
    //mp.trigger("ClientOnRangeChange", id, val);
    alt.emit('face:client-onChange', id, val);
}
