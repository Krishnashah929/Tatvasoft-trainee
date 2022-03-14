// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
//progressive form js//
form = document.getElementById('Form');
fieldsets = document.querySelectorAll('fieldset');
back = document.getElementById('back');
back1 = document.getElementById('back1');
back2 = document.getElementById('back2');
nextORsubmit = document.getElementById('nextORsubmit');

let i = 0;
function next() {
    if (i < fieldsets.length - 1) {
        fieldsets[i].style.display = 'none';
        fieldsets[i + 1].style.display = 'block';
        back.style.display = 'inline';
        $("#progressbar li").eq($("fieldset").index(fieldsets[i + 1])).addClass("active");
        i++;
        // set required on current fieldset inputs, except if they're checkboxes
        fieldsets[i].querySelectorAll('input:not([type="checkbox"])').forEach(el => {
            el.required = true;
        })
    }
}

back.addEventListener('click', () => {
    console.log('going back a step');
    fieldsets[i].style.display = 'none';
    fieldsets[i - 1].style.display = 'block';
    $("#progressbar li").eq($("fieldset").index(fieldsets[i])).removeClass("active");
    i--;
    // remove required on inputs from the next fieldset that we've just hide
    fieldsets[i + 1].querySelectorAll('input:not([type="checkbox"])').forEach(el => {
        el.required = false;
    })
    // remove back button when you go back to the first step
    if (i == 0) {
        back.style.display = 'none';
    }
})
back1.addEventListener('click', () => {
    console.log('going back a step');
    fieldsets[i].style.display = 'none';
    fieldsets[i - 1].style.display = 'block';
    $("#progressbar li").eq($("fieldset").index(fieldsets[i])).removeClass("active");
    i--;

    fieldsets[i + 1].querySelectorAll('input:not([type="checkbox"])').forEach(el => {
        el.required = false;
    })

    if (i == 0) {
        back1.style.display = 'none';
    }
})
back2.addEventListener('click', () => {
    console.log('going back a step');
    fieldsets[i].style.display = 'none';
    fieldsets[i - 1].style.display = 'block';
    $("#progressbar li").eq($("fieldset").index(fieldsets[i])).removeClass("active");
    i--;

    fieldsets[i + 1].querySelectorAll('input:not([type="checkbox"])').forEach(el => {
        el.required = false;
    })

    if (i == 0) {
        back2.style.display = 'none';
    }
})

// schedulesubmit js
function zipsubmit() {
    var data = {};
    data.ZipcodeValue = $("#inputnumber").val();
    console.log(data);
    $.ajax({
        type: 'POST',
        url: '/CustomerPages/ValidZip',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: data,
        success: function (result) {
            if (result.value == "true") {
                next();
            }
            else {
                alert("Zipcode  does not exsit !!!");
            }
        },
        error: function () {
            alert('Failed to receive the Data');
            console.log('Failed ');
        }
    })
}

function schedulesubmit() {
    var data = $("#scheduleform").serialize();
    console.log(data);
    $.ajax({
        type: 'POST',
        url: '/CustomerPages/Scheduledetails',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: data,
        success: function (result) {
            if (result.value == "true") {
                getaddress();
                next();
                /*alert("valid schedule details");*/
            }
        },
        error: function () {
            alert('Failed to receive the Data');
            console.log('Failed ');
        }
    })
}

function getaddress() {
    console.log("get address ");
    $.ajax({
        type: 'GET',
        url: '/CustomerPages/LoadAddress',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (result) {
            console.log(result[0].AddressId);
            var address = $("#gettingalladdress");
            address.empty();
            console.log(result);
            for (let j = 0; j < result.length; j++) {
                if (result[j].isDefault) {
                    address.append(' <div class="row"> <label for="address' + j + '" class="address - booknow - label"> <input type="radio" class="address-booknow-radio-btn " asp-for="AddressId" checked id="address' + j + '" name="address" value="' + result[j].addressId + '"> Address: ' + result[j].addressLine1 + " " + result[j].addressLine2 + ", " + result[j].city + " " + result[j].postalCode + ' </label > <div class="row">  <label for="address' + j + '" class="address - booknow - label - 1 ml-3"> Phone Number: ' + result[j].mobile + '  </label> </div> </div > ');
                }
                else {
                    address.append(' <div class="row"> <label for="address' + j + '" class="address - booknow - label"> <input type="radio" class="address-booknow-radio-btn " asp-for="AddressId" checked id="address' + j + '" name="address" value="' + result[j].addressId + '"> Address: ' + result[j].addressLine1 + " " + result[j].addressLine2 + ", " + result[j].city + " " + result[j].postalCode + ' </label > <div class="row">  <label for="address' + j + '" class="address - booknow - label - 1 ml-3"> Phone Number: ' + result[j].mobile + '  </label> </div> </div > ');
                }
            }
        },
        error: function () {
            alert('Failed to receive the Data');
            console.log('Failed ');
        }
    })
}

function addresssubmit() {
    var data = {};
    data.addressLine1 = document.getElementById("addStreet").value;
    data.addressLine2 = document.getElementById("addHouseNumber").value;
    data.postalCode = document.getElementById("addPostalCode").value;
    data.city = document.getElementById("addCity").value;
    data.mobile = document.getElementById("addPhoneNumber").value;
    console.log("get address new");
    $.ajax({
        type: 'POST',
        url: '/CustomerPages/NewAddress',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: data,
        success: function (result) {
            if (result.value == "true") {
                getaddress();

                alert("address is valid");
            }
            else {
                alert("address is invalid");
            }
        },
        error: function () {
            alert('Failed to receive the Data');
            console.log('Failed ');
        }
    })
}

function afteraddress() {
    next();
    alert("valid address details");
}

function completebooking() {
    var data = {};

    var extrahours = 0;
    var c = document.getElementById("Insidecabinets");
    var f = document.getElementById("Insidefridge");
    var o = document.getElementById("Insideoven");
    var l = document.getElementById("Insidelaundry");
    var w = document.getElementById("Insidewindows");
    data.cabinet = false;
    data.fridge = false;
    data.oven = false;
    data.laundry = false;
    data.window = false;

    if (c.checked) {
        extrahours += 0.5;
        data.cabinet = true;
    }
    if (f.checked) {
        extrahours += 0.5;
        data.fridge = true;
    }
    if (o.checked) {
        extrahours += 0.5;
        data.oven = true;
    }
    if (l.checked) {
        extrahours += 0.5;
        data.laundry = true;
    }
    if (w.checked) {
        extrahours += 0.5;
        data.window = true;
    }
    data.zipcodeValue = document.getElementById("inputnumber").value;
    data.serviceDate = document.getElementById("servicestartdate").value;
    data.serviceTime = document.getElementById("serviceTime").value;
    data.serviceHours = parseFloat(document.getElementById("serviceHours").value);
    data.extrahours = extrahours;
    data.subTotal = parseFloat(data.extrahours + data.serviceHours);
    data.totalCost = data.subTotal * 25;
    data.comments = document.getElementById("comments").value;
    var h = document.getElementById("checkhaspet");
    data.hasPets = false;
    if (h.checked) {
        data.hasPets = true;
    }
    data.paymentDue = false;
    data.paymentDone = true;
    data.addressId = $("#gettingalladdress div input[name='address']:checked").val();

    $.ajax({
        type: 'post',
        url: '/CustomerPages/Completebooking',
        contenttype: 'application/x-www-form-urlencoded; charset=utf-8',
        data: data,
        success: function (result) {
            if (result.value == "false") {
            }
        },
        error: function () {
            alert('failed to receive the data');
            console.log('failed ');
        }
    })
}

function etc1change() {
    $("#Insidecabinets").checked
    var cb = document.getElementById("Insidecabinets");
    if (cb.checked) {
        $("#etc-1").removeClass("d-none");
        $("#etc1").removeClass("d-none");
    }
    else {
        $("#etc-1").addClass("d-none");
        $("#etc1").addClass("d-none");
    }
}
function etc2change() {
    $("#Insidefridge").checked
    var fd = document.getElementById("Insidefridge");
    if (fd.checked) {
        $("#etc-2").removeClass("d-none");
        $("#etc2").removeClass("d-none");
    }
    else {
        $("#etc-2").addClass("d-none");
        $("#etc2").addClass("d-none");
    }
}
function etc3change() {
    $("#Insideoven").checked
    var ov = document.getElementById("Insideoven");
    if (ov.checked) {
        $("#etc-3").removeClass("d-none");
        $("#etc3").removeClass("d-none");
    }
    else {
        $("#etc-3").addClass("d-none");
        $("#etc3").addClass("d-none");
    }
}
function etc4change() {
    $("#Insidelaundry").checked
    var ly = document.getElementById("Insidelaundry");
    if (ly.checked) {
        $("#etc-4").removeClass("d-none");
        $("#etc4").removeClass("d-none");
    }
    else {
        $("#etc-4").addClass("d-none");
        $("#etc4").addClass("d-none");
    }
}
function etc5change() {
    $("#Insidewindows").checked
    var win = document.getElementById("Insidewindows");
    if (win.checked) {
        $("#etc-5").removeClass("d-none");
        $("#etc5").removeClass("d-none");
    }
    else {
        $("#etc-5").addClass("d-none");
        $("#etc5").addClass("d-none");
    }
}
function datechange() {
    var date = document.getElementById("servicestartdate").value;
    document.getElementById("servicecarddate").innerHTML = date;
}

function timechange() {
    var time = document.getElementById("serviceTime").value;
    document.getElementById("servicecardtime").innerHTML = time;
}

function hourschange() {
    var hour = document.getElementById("serviceHours").value;
    document.getElementById("servicecardhour").innerHTML = hour;
}
