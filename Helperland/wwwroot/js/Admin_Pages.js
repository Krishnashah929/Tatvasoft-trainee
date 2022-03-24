$(document).ready(function () {
    debugger;
    $.ajax({
        url: '/AdminPages/UserManagement',
        type: 'GET',
        success: function (response) {
            $('#Main-content').html(response);
            $('#usertable').DataTable({
                "searching": false,
                //"dom": '<"bottom"i>rt<"top"flp><"clear">',
                'columnDefs': [{
                    'targets': [6],
                    'orderable': false,
                }],
            });
        }
    });
});
function loadusermanage() {
    debugger;
    $.ajax({
        url: '/AdminPages/UserManagement',
        type: 'GET',
        success: function (response) {
            $('#Main-content').html(response);
            $('#usertable').DataTable({
                "searching": false,
                //  "dom": '<"top"i>rt<"bottom"flp><"clear">',
                columnDefs: [{
                    targets: [0],
                    orderData: [0, 1]
                }, {
                    targets: [1],
                    orderData: [1, 0]
                }, {
                    targets: [4],
                    orderData: [4, 0]
                }]
            });
        }
    });
}
function loaduserrequest() {
    debugger;
    $.ajax({
        url: '/AdminPages/UserRequest',
        type: 'GET',
        success: function (response) {
            $('#Main-content').html(response);
            $('#requesttable').DataTable({
                "searching": false,
                //  "dom": '<"top"i>rt<"bottom"flp><"clear">',
                columnDefs: [{
                    targets: [0],
                    orderData: [0, 1]
                }, {
                    targets: [1],
                    orderData: [1, 0]
                }, {
                    targets: [4],
                    orderData: [4, 0]
                }]
            });
        }
    });
}

function Approveuser(or) {
    debugger;
    var id = or.getAttribute("data-id");
    $.ajax({
        url: '/AdminPages/ApproveUser',
        type: 'GET',
        data: { "id": id },
        success: function (response) {
            $('#Model1').html(response);
            $('#ApproveUsers').modal("show");
        }
    });
}
function finalapprove(or) {
    debugger;
    var data = {};
    data.userId = document.getElementById("id").value;
    $.ajax({
        url: '/AdminPages/FinalApproveUser',
        type: 'POST',
        data: data,
        success: function (response) {
            if (result.value == "true") {
                loadusermanage();
            }
            else {
                alert("Invalid");
            }
        }
    });
}

function Deleteuser(or) {
    debugger;
    var id = or.getAttribute("data-id");
    $.ajax({
        url: '/AdminPages/DeactivateUser',
        type: 'POST',
        data: { "id": id },
        success: function (response) {
            if (result.value == "true") {
                loadusermanage();
            }
            else {
                alert("Invalid");
            }
        }
    });
}
function Activeuser(or) {
    var id = or.getAttribute("data-id");
    $.ajax({
        url: '/AdminPages/ActivateUser',
        type: 'POST',
        data: { "id": id },
        success: function (response) {
            if (result.value == "true") {
                loadusermanage();
            }
            else {
                alert("Invalid");
            }
        }
    });
}

function editservice(sd) {
    debugger;
    var id = sd.getAttribute("data-id");
    $.ajax({
        url: '/AdminPages/EditServiceModel',
        type: 'GET',
        data: { "id": id },
        success: function (response) {
            $('#Model2').html(response);
            $('#editservice').modal("show");
        }
    });
}
function updateservice() {
    var data = {};
    data.serviceRequestId = document.getElementById("id").value;
    data.serviceDate = document.getElementById("ServiceStartDate").value;
    data.serviceTime = document.getElementById("ServiceTime").value;
    data.addressLine1 = document.getElementById("AddressLine1").value;
    data.addressLine2 = document.getElementById("AddressLine2").value;
    data.zipCode = document.getElementById("ZipCode").value;
    data.city = document.getElementById("City").value;
    data.comments = document.getElementById("Comments").value;
    $.ajax({
        url: '/AdminPages/UpdateServiceModel',
        type: 'POST',
        data: data,
        success: function (result) {
            if (result.value == "true") {
                loaduserrequest();
            }
            else {
                alert("Invalid");
            }
        }
    });
}

function cancel(cm) {
    debugger;
    var id = cm.getAttribute("data-id");
    $.ajax({
        url: '/AdminPages/CancelServiceModel',
        type: 'GET',
        data: { "id": id },
        success: function (response) {
            $('#Model3').html(response);
            $('#CancelModal').modal("show");
        }
    });
}
function CancelRequest() {
    debugger;
    var data = {};
    data.serviceRequestId = document.getElementById("id").value;
    data.comments = document.getElementById("cancelrequest").value;
    data.comments = document.getElementById("Comments").value;
    $.ajax({
        url: '/AdminPages/CancelRequestModel',
        type: 'POST',
        data: data,
        success: function (result) {
            if (result.value == "true") {
                loaduserrequest();
            }
            else {
                alert("Invalid");
            }
        }
    });
}

function searchservice() {
    debugger;
    var data = {};
    data.name = document.getElementById("name").value;
    data.mobile = document.getElementById("phonenumber").value;
    data.zipCode = document.getElementById("zipcode").value;
    data.userTypeId = document.getElementById("usertype").value;
    data.createdDate = document.getElementById("createdate").value;
    $.ajax({
        type: 'GET',
        url: '/AdminPages/UserManagement',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: data,
        success: function (response) {
            $('#Main-content').html(response);
        }
    });
}
function searchrequest() {
    debugger;
    var data = {};
    data.serviceRequestId = document.getElementById("ServiceID").value;
    data.custName = document.getElementById("name").value;
    data.name = document.getElementById("custName").value;
    data.status = document.getElementById("status").value;
    data.zipCode = document.getElementById("zipcode").value;
    data.serviceStartDate = document.getElementById("startdate").value;

    $.ajax({
        type: 'GET',
        url: '/AdminPages/UserRequest',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: data,
        success: function (response) {
            $('#Main-content').html(response);
        }
    });
}

function clearservice() {
    debugger;
    loadusermanage();
}
function clearrequest() {
    debugger;
    loaduserrequest();
}
//for downloading the excel file
function ExportToExcel1(type, fn, dl) {
    var elt = document.getElementById('usertable');
    var wb = XLSX.utils.table_to_book(elt, { sheet: "sheet1" });
    return dl ?
        XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
        XLSX.writeFile(wb, fn || ('Servicehistorycustomer.' + (type || 'xlsx')));
}
function ExportToExcel2(type, fn, dl) {
    var elt = document.getElementById('usertable');
    var wb = XLSX.utils.table_to_book(elt, { sheet: "sheet1" });
    return dl ?
        XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
        XLSX.writeFile(wb, fn || ('Servicehistorycustomer.' + (type || 'xlsx')));
}



