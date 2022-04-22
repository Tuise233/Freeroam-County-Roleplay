$(document).ready(function() {
    $('.ui.dropdown').dropdown();
    
    $("#charcreation_back").click(function() {
        //mp.trigger("ClientCharCreation2Back");
        alt.emit('face2:client-backCreate');
    });
    
    $("#charcreation_next").click(function() {
        //mp.trigger("ClientCharCreation2Next");
        alt.emit('face2:client-nextCreate');
    });

    $("#charcreation_head").click(function() {
        alt.emit('face2:client-cameraTo', 1);
    });
    
    $("#charcreation_body").click(function() {
        alt.emit('face2:client-cameraTo', 0);
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
    alt.emit('face2:client-onChange', id, val);
}

function LoadFaceFeatures(arr_data) {
    var data = JSON.parse(arr_data);
    for(let i = 0; i <= 20; i++) {
        $("#" + i).range({
            min: 0,
            max: 19,
            start: data[i].FaceFeatures,
            step: 1,
            onChange: function(val) {
                alt.emit('face2:client-setFaceFeature', i, val);
            },
            smooth: false
        });
    };
}

alt.on('face2:view-loadFeature', LoadFaceFeatures);