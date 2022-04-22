$(document).ready(function() {
    $('#btn_newchar').click(function() {
        //mp.trigger("CreateCharacter");
        alt.emit('character:client-createCharacter');
    });
});
function LoadCharacters(chars) {
    if(chars){
        var character = JSON.parse(chars);
        if(character.length > 0) 
        {
            for(let i = 0; i < character.length; i++) 
            {
                $('#characters').append('<div class="panel panel-inventory"><div class="panel-heading"><h3 class="panel-title"><center>角色 <b>'+character[i].Name+'</b></h3></div> <div class="panel-footer panel-male"><table class="table"><tbody><tr><td>级别</td><td>'+character[i].Level+'</td></tr><tr><td>经验值</td><td>'+character[i].Exp+' / '+character[i].Exp_Max+'</td></tr><tr><td>现金</td><td>$'+character[i].Money+'</td></tr><tr><td>银行存款</td><td>$'+character[i].Bank+'</td></tr></tbody></table><button href="#" class="btn btn-info " role="button" id="preview_'+character[i].ID+'">预览</button><button href="#" class="btn btn-success " role="button" id="select_'+character[i].ID+'">选择</button></div></div>');
                $('#preview_'+character[i].ID).click(function() {
                    //mp.trigger("ClientPreviewCharacterID", character[i].ID);
                    alt.emit('character:client-previewCharacter', character[i].ID);
                });
                $('#select_'+character[i].ID).click(function() {
                    //mp.trigger("SelectCharacter", character[i].ID);
                    alt.emit('character:client-selectCharacter', character[i].ID);
                });
            }
        }
    }
}

alt.on('character:view-loadCharacters', LoadCharacters);