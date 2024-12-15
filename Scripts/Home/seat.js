
var total = 0;

document.getElementById("total-text").innerText = total;
document.getElementById("total").value = total;
const calcTotal = (e, money) => {
    var seat = e.target.id.replace("seat_check", "") - 0
    if (!document.getElementById("seat" + seat).checked) {
        total += money;
    }
    else {
        total -= money
    }
    document.getElementById("total-text").innerText = total;
    document.getElementById("total").value = total;
}

const chooseSweetBox = (e, money) => {
    var seat1 = e.target.id.replace("seat_check", "") - 0
    var seat2 = seat1 % 2 == 0 ? seat1 - 1 : seat1 + 1
    document.getElementById("seat" + seat2).checked = !document.getElementById("seat" + seat1).checked
    document.getElementById("seat" + seat1).checked = document.getElementById("seat" + seat1).checked
    if (!document.getElementById("seat" + seat1).checked) {
        total += money;
    }
    else {
        total -= money
    }
    document.getElementById("total").innerText = total;
}

const mySubmit = () => {
    form = document.querySelector("#myForm")
    if (document.querySelectorAll('input[id^="seat"]:checked').length == 0) {
        alert("PLease select at least 1 seat!")
    }
    else if (document.querySelectorAll('input[id^="seat"]:checked') > 8) {
        alert("You can only select up to 8 seats at a time")
    }
    else if (!form.reportValidity()) {

    }
    else {
        form.submit()
    }
}

{/*$.ajax({*/ }
{/*type: 'post',*/ }
{/*url: `/home/payment`,*/ }
{/*data: {*/ }
{/*    id: id*/ }
{/*},*/ }
{/*success: function (response) {*/ }
{/*    if (response == 'OK') {*/ }
{/*        location.href = '/gallery/index';*/ }
{/*    }*/ }
{/*}*/ }
{/*})*/ }
