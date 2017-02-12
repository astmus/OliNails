function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = document.cookie;
    var from = decodedCookie.indexOf(name) + name.length;
    var to = decodedCookie.indexOf("&", from);
    return to < 0 ? decodedCookie.substring(from) : decodedCookie.substring(from, to);
}

function showEnterPhonePrompt(dateId) {
    var phone = getCookie('phone')
    console.log(phone);
    console.log(document.cookie);
    if (phone.length < 13)
        phone = '+38';
    var phone = prompt("Что бы изменить или отменить запись введите ваш номер телефона", phone);
    if (phone.length > 4) {
        PageMethods.CheckEditNailDate(dateId, phone, onSuccess, onError);
    }
}

function onSuccess(result) {
    window.location.href = "SelectSercvices.aspx";
}

function onError(result) {
    if (confirm("К сожалению номер телефона не совпал.\n Возможно вы выбрали не свою запись.\n Если все верно, свяжитесь с нами.\n Позвонить Оле?") == true) {
        window.open("tel:+380953464708");
    }
}