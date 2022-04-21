/// <reference types='@altv/types-client' />
/// <reference types='@altv/types-natives' />
import * as alt from 'alt';
import * as native from 'natives';

const maxStringLength = 40;

alt.on('SendSuccessNotification', s => show(s, true, 1, 80, [0, 0, 0, 50]));
alt.on('SendErrorNotification', s => show(s, true, 1, 90, [0, 0, 0, 50]));
alt.on('SendInfoNotification', s => show(s, false, 1, 25, [0, 0, 0, 0]));
alt.on('SendWarnNotification', s => show(s, false, 1, 4, [0, 0, 0, 0]));

alt.onServer('SendSuccessNotification', s => show(s, true, 1, 80, [0, 0, 0, 50]));
alt.onServer('SendErrorNotification', s => show(s, true, 1, 90, [0, 0, 0, 50]));
alt.onServer('SendInfoNotification', s => show(s, false, 1, 25, [0, 0, 0, 0]));
alt.onServer('SendWarnNotification', s => show(s, false, 1, 4, [0, 0, 0, 0]));




alt.onServer('notifications:show', show);
alt.onServer('notifications:showWithPicture', showWithPicture);
alt.onServer('notifications:removeone', Remove);

alt.on('notification:show', show);
alt.on('notification:showWithPicture', showWithPicture);
alt.on('notification:removeone', Remove);

/*
	show
	展示Notification

	message: String
	flashing: Bool
	textColor: Int
	bgColor: Int,
	flashColor: Int[4] (RGBA)
*/
export function show(
	message,
	flashing = false,
	textColor = -1,
	bgColor = -1,
	flashColor = [0, 0, 0, 50]
) {
	if (textColor > -1) {
		native.setColourOfNextTextComponent(textColor);
	}

	if (bgColor > -1) {
		native.thefeedSetNextPostBackgroundColor(bgColor);
	}

	if (flashing) {
		native.thefeedSetAnimpostfxColor(flashColor[0], flashColor[1], flashColor[2], flashColor[3]);
	}

	native.beginTextCommandThefeedPost('CELL_EMAIL_BCON');

	for (let i = 0, msgLen = message.length; i < msgLen; i += maxStringLength) {
		native.addTextComponentSubstringPlayerName(message.substr(i, Math.min(maxStringLength, message.length - i)));
	}
	let i = native.endTextCommandThefeedPostTicker(flashing, true);

	alt.emit('sound:playsound', 3);

	//延后删除
	if (catchNotify) {
		alt.emitServer('catch:notify', i);
		setTimeout(() => {
			native.thefeedRemoveItem(i);
		}, 6000);
	}
	else {
		setTimeout(() => {
			native.thefeedRemoveItem(i);
		}, 6000);
	}

	return i	;
}

/*
	showWithPicture
	展示带图片的Notification
    
	title: String
	sender: String
	message: String
	notifPic: String (https://wiki.rage.mp/index.php?title=Notification_Pictures)
	iconType: Int
	flashing: Bool
	textColor: Int,
	bgColor: Int,
	flashColor: Int[4] (RGBA)
*/
export function showWithPicture(
	title,
	sender,
	message,
	notifPic,
	iconType = 0,
	flashing = false,
	textColor = -1,
	bgColor = -1,
	flashColor = [0, 0, 0, 50],
	audio = 3
) {
	if (textColor > -1) {
		native.setColourOfNextTextComponent(textColor);
	}

	if (bgColor > -1) {
		native.thefeedSetNextPostBackgroundColor(bgColor);
	}

	if (flashing) {
		native.thefeedSetAnimpostfxColor(flashColor[0], flashColor[1], flashColor[2], flashColor[3]);
	}

	native.beginTextCommandThefeedPost('CELL_EMAIL_BCON');
	for (let i = 0, msgLen = message.length; i < msgLen; i += maxStringLength) {
		native.addTextComponentSubstringPlayerName(message.substr(i, Math.min(maxStringLength, message.length - i)));
	}

	native.endTextCommandThefeedPostMessagetext(notifPic, notifPic, flashing, iconType, title, sender);

	alt.emit('sound:playsound', audio);

	let i = native.endTextCommandThefeedPostTicker(false, true);
	
	//延后删除
	if (catchNotify) {
		alt.emitServer('catch:notify', i);
		setTimeout(() => {
			native.thefeedRemoveItem(i);
		}, 20000);
	}
	else {
		setTimeout(() => {
			native.thefeedRemoveItem(i);
		}, 20000);
	}

	return i;
}

var catchNotify = false;
var ctimer = null;
alt.onServer('catch:notify', () => {
	catchNotify = true;
	if (ctimer) {
		clearTimeout(ctimer);
		ctimer = null;
	}
	ctimer = setTimeout(() => {
		catchNotify = false;
		if (ctimer) {
			clearTimeout(ctimer);
			ctimer = null;
		}
	}, 500);
});


export function Remove(id) {
	native.thefeedRemoveItem(id);
}
