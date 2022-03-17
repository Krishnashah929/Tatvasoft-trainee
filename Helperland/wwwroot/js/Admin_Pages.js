$(document).ready(function () {
    debugger;
    $.ajax({
        url: '/AdminPages/UserManagement',
        type: 'GET',
        success: function (response) {
            $('#Main-content').html(response);
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
            //$('#usertable').DataTable({
            //    "searching": false,
            //    //  "dom": '<"top"i>rt<"bottom"flp><"clear">',
            //    columnDefs: [{
            //        targets: [0],
            //        orderData: [0, 1]
            //    }, {
            //        targets: [1],
            //        orderData: [1, 0]
            //    }, {
            //        targets: [4],
            //        orderData: [4, 0]
            //    }]
            //});
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

//for downloading the excel file
function ExportToExcel1(type, fn, dl) {
    var elt = document.getElementById('usertable');
    var wb = XLSX.utils.table_to_book(elt, { sheet: "sheet1" });
    return dl ?
        XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
        XLSX.writeFile(wb, fn || ('Servicehistorycustomer.' + (type || 'xlsx')));
}