$(document).ready(function () {

    $("#charcreation_back").click(function () {
        mp.trigger("ClientCharCreation3Back");
    });

    $("#charcreation_next").click(function () {
        mp.trigger("ClientCharCreation3Next");
    });

    $("#charcreation_head").click(function () {
        mp.trigger("cameraPointTo", 1);
    });

    $("#charcreation_body").click(function () {
        mp.trigger("cameraPointTo", 0);
    });

    $('#range_rotation').range({
        min: 0,
        max: 360,
        start: 100,
        step: 20,
        smooth: false,
        onChange: function (val, data) {
            OnRangeChange("range_rotation", val, null)
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
});

function OnRangeChange(id, val, data) {
    console.log(id, val);
    mp.trigger("ClientOnRangeChange", id, val);
}

function LoadClothing(arr_data) {
    var data = JSON.parse(arr_data);
    $('#traje').range({
        min: 0,
        max: 12,
        start: data.Top,
        smooth: false,
        onChange: function (val, data) {
            mp.trigger("ClientSetTraje", val);
            $('#traje-display').html(val)
        }
    });
    $('#top').range({
        min: 0,
        max: 10,
        start: data[0].Top,
        smooth: false,
        onChange: function (val, data) {
            mp.trigger("ClientSetTorso", val);
        }
    });
    $('#pants').range({
        min: 0,
        max: 10,
        start: data[0].Pants,
        smooth: false,
        onChange: function (val, data) {
            mp.trigger("ClientSetPants", val);
        }
    });
    $('#shoes').range({
        min: 0,
        max: 10,
        start: data[0].Shoes,
        smooth: false,
        onChange: function (val, data) {
            mp.trigger("ClientSetShoes", val);
        }
    });
}