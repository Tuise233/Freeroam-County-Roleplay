$(document).ready(function() {
    $('.ui.dropdown').dropdown();
    
    $("#charcreation_back").click(function() {
        //mp.trigger("ClientCharCreation3Back");
        alt.emit('face3:client-backCreate');
    });
    
    $("#charcreation_next").click(function() {
        //mp.trigger("ClientCharCreation3Next");
        alt.emit('face3:client-nextCreate');
    });

    $("#charcreation_head").click(function() {
        alt.emit('face3:client-cameraTo', 1);
    });
    
    $("#charcreation_body").click(function() {
        alt.emit('face3:client-cameraTo', 0);
    });

    $('#range_rotation').range({
        min: 0,
        max: 360,
        start: 100,
        step: 20,
        smooth: false,
        onChange: function(val, data) { OnRangeChange("range_rotation", val, null) }
    });
    $('#range_elevation').range({
        min: -2,
        max: 1,
        start: 0,
        step: 1,
        smooth: false,
        onChange: function(val, data) { OnRangeChange("range_elevation", val, null) }
    });
});

function OnRangeChange(id, val, data) {
    console.log(id, val);
    alt.emit('face3:client-onChange', id, val);
}

function LoadClothing(arr_data) {
    var data = JSON.parse(arr_data);
    $('#traje').range({
        min: 0,
        max: 12,
        start: data.Top,
        smooth: false,
        onChange: function(val, data) {
            //mp.trigger("ClientSetTraje", val);
            alt.emit('face3:client-setClothes', 'traje', val);
        }
    });
    $('#top').range({
        min: 0,
        max: 10,
        start: data[0].Top,
        smooth: false,
        onChange: function(val, data) {
            //mp.trigger("ClientSetTorso", val);
            alt.emit('face3:client-setClothes', 'torso', val);
        }
    });
    $('#pants').range({
        min: 0,
        max: 10,
        start: data[0].Pants,
        smooth: false,
        onChange: function(val, data) {
            //mp.trigger("ClientSetPants", val);
            alt.emit('face3:client-setClothes', 'pants', val);
        }
    });
    $('#shoes').range({
        min: 0,
        max: 10,
        start: data[0].Shoes,
        smooth: false,
        onChange: function(val, data) {
            //mp.trigger("ClientSetShoes", val);
            alt.emit('face3:client-setClothes', 'shoes', val);
        }
    });
}

alt.on('face3:view-loadClothing', LoadClothing);