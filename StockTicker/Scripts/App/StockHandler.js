// Copyright(c) Daniel Veintimilla 2016.

function StockHandler() { }

StockHandler.GetUserStockList = function () {
    var token = CommonUtils.GetItemFromLocalStorage(UserLoginHandler.DefaultTokenStorageKey);
    if (CommonUtils.IsValid(token)) {
        CommonUtils.BlurLoadingScreen();
        $.ajax(
            'http://localhost/CSharpAssignment/StockExchangeWS.asmx/GetUserStock',
            {
                cache: true,
                async: true,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ token: token }),
                success: function (json) { StockHandler.InitStockUserList(json); },
                error: function (error) { StockHandler.FailureStockUserList(error); }
            });
    }
}

StockHandler.GetAllExceptUserStock = function () {
    var token = CommonUtils.GetItemFromLocalStorage(UserLoginHandler.DefaultTokenStorageKey);
    if (CommonUtils.IsValid(token)) {
        $.ajax(
            'http://localhost/CSharpAssignment/StockExchangeWS.asmx/GetAllExceptUserStock',
            {
                cache: true,
                async: true,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ token: token }),
                success: function (json) { StockHandler.InitStockAllListInModal(json); },
                error: function (error) { StockHandler.FailureStockUserList(error); }
            });
    }
}

StockHandler.InitStockAllListInModal = function (json) {
    var jsonObj = JSON.parse(json.d);
    if (CommonUtils.IsNotUndefinedAndNull(jsonObj)) {
        if (CommonUtils.IsValid(jsonObj.Messages)) {
            MessageProcessor.ProcessMessages(jsonObj.Messages);
        }
        else {
            var jsonModel = jsonObj.Model;
            if (CommonUtils.IsNotUndefinedAndNull(jsonModel)) {
                var modelStockList = jsonModel.StockList;
                if (CommonUtils.IsNotUndefinedAndNull(modelStockList)) {
                    gridData = StockMapper.MapStockListToStockGridData(modelStockList);

                    var table = $('#all-stock-list-grid').DataTable({
                        data: gridData,
                        "searching": false,
                        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                        "pageLength": 10,
                        select: 'single',
                        ordering: false,
                        columns: [
                            { title: "StockCode" },
                            { title: "StockName" },
                            {
                                title: "Price",
                                "render": function (data, type, full) {
                                    var options = {
                                        symbol: "U$",
                                        decimal: ".",
                                        thousand: ",",
                                        precision: 2,
                                        format: "%s%v"
                                    };
                                    var formatted = accounting.formatMoney(data, options)

                                    return formatted;
                                }
                            }
                        ]
                    });
                }
            }
        }
    }
}

StockHandler.InitStockUserList = function (json) {

    var jsonObj = JSON.parse(json.d);
    if (CommonUtils.IsNotUndefinedAndNull(jsonObj)) {
        if (CommonUtils.IsValid(jsonObj.Messages)) {
            MessageProcessor.ProcessMessages(jsonObj.Messages);
        }
        else {
            var jsonModel = jsonObj.Model;
            if (CommonUtils.IsNotUndefinedAndNull(jsonModel)) {
                var modelStockList = jsonModel.StockList;
                if (CommonUtils.IsNotUndefinedAndNull(modelStockList)) {
                    gridData = StockMapper.MapStockListToStockGridData(modelStockList);

                    $('#stock-list-grid').DataTable({
                        data: gridData,
                        "searching": false,
                        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
                        "pageLength": 10,
                        select: 'single',
                        columns: [
                            { title: "StockCode" },
                            { title: "StockName" },
                            {
                                title: "Price",
                                "render": function (data, type, full) {
                                    var options = {
                                        symbol: "U$",
                                        decimal: ".",
                                        thousand: ",",
                                        precision: 2,
                                        format: "%s%v"
                                    };
                                    var formatted = accounting.formatMoney(data, options)

                                    return formatted;
                                }
                            }
                        ]
                    });

                    StockHandler.BindStockEvents();
                    CommonUtils.RemoveLoadingScreen();
                }
            }
        }
    }
}

StockHandler.BindStockEvents = function () {
    var table = StockHandler.GetStockGridTable();

    table.on('select', function (e, dt, type, indexes) {
        if (type === 'row') {
            var data = table.rows(indexes).data();
            var stockCode = data[0][0];
            if (CommonUtils.IsValid(stockCode)) {
                CommonUtils.EnableHtmlComponentByID('delete-record-grid-btn', true);
            }
        }
    });

    $('#myModal').on(
        'shown.bs.modal',
        {},
        function (event) {
            StockHandler.GetAllExceptUserStock();
        });

    $("#myModal").on('hidden.bs.modal', function () {
        $(this).data('bs.modal', null);
        StockHandler.DestroyTable('all-stock-list-grid');
    });
}

StockHandler.FailureStockUserList = function (error) {
    CommonUtils.EnableHtmlComponentByID('delete-record-grid-btn', false);
    CommonUtils.RemoveLoadingScreen();
    MessageProcessor.ProcessFailureRequest(error);
}

StockHandler.SaveSelectedUserStock = function (event) {
    var table = StockHandler.GetAllStockGridTable();
    if (CommonUtils.IsNotUndefinedAndNull(table)) {
        var selectedRow = table.rows({ selected: true }).data();
        if (CommonUtils.IsValid(selectedRow)) {
            var stockCode = selectedRow[0][0];
            if (CommonUtils.IsValid(stockCode)) {
                CommonUtils.BlurLoadingScreen();
                StockHandler.SaveUserStockCode(stockCode);
            }
        }
        else {
            alert('Please select one row to delete')
        }
    }
}

StockHandler.RefreshUserGridData = function () {
    var token = CommonUtils.GetItemFromLocalStorage(UserLoginHandler.DefaultTokenStorageKey);
    if (CommonUtils.IsValid(token)) {
        $.ajax(
            'http://localhost/CSharpAssignment/StockExchangeWS.asmx/GetUserStock',
            {
                cache: true,
                async: true,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ token: token }),
                success: function (json) { StockHandler.ProcessRefreshUserGridData(json); },
                error: function (error) { StockHandler.FailureStockUserList(error); }
            });
    }
}

StockHandler.ProcessRefreshUserGridData = function (json) {
    CommonUtils.EnableHtmlComponentByID('delete-record-grid-btn', false);
    var jsonObj = JSON.parse(json.d);
    if (CommonUtils.IsNotUndefinedAndNull(jsonObj)) {
        if (CommonUtils.IsValid(jsonObj.Messages)) {
            CommonUtils.RemoveLoadingScreen();
            MessageProcessor.ProcessMessages(jsonObj.Messages);
        }
        else {
            var modelStockList = jsonObj.Model.StockList;
            if (CommonUtils.IsNotUndefinedAndNull(modelStockList)) {
                gridData = StockMapper.MapStockListToStockGridData(modelStockList);

                var table = StockHandler.GetStockGridTable();
                if (CommonUtils.IsNotUndefinedAndNull(table)) {
                    table.clear();
                    table.rows.add(gridData);
                    table.draw();
                }
            }
        }
    }
}

StockHandler.SaveUserStockCode = function (stockCode) {
    var token = CommonUtils.GetItemFromLocalStorage(UserLoginHandler.DefaultTokenStorageKey);
    if (CommonUtils.IsValid(token)) {
        CommonUtils.BlurLoadingScreen();
        $.ajax(
            'http://localhost/CSharpAssignment/StockExchangeWS.asmx/AddUserStockItem',
            {
                cache: true,
                async: true,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ token: token, stockCode: stockCode }),
                success: function (json) { StockHandler.ProcessSaveUserStockCode(json); },
                error: function (error) { StockHandler.FailureStockUserList(error); }
            });
    }
}

StockHandler.ProcessSaveUserStockCode = function (json) {
    var jsonObj = JSON.parse(json.d);
    if (CommonUtils.IsNotUndefinedAndNull(jsonObj)) {
        if (CommonUtils.IsValid(jsonObj.Messages)) {
            CommonUtils.RemoveLoadingScreen();
            MessageProcessor.ProcessMessages(jsonObj.Messages);
        }
        else {
            StockHandler.RefreshUserGridData();
            $('#myModal').modal('hide');
            CommonUtils.RemoveLoadingScreen();
        }
    }

}

StockHandler.DeleteRecord = function (event) {
    var table = StockHandler.GetStockGridTable();
    if (CommonUtils.IsNotUndefinedAndNull(table)) {
        var selectedRow = table.rows({ selected: true }).data();
        if (CommonUtils.IsValid(selectedRow)) {
            var stockCode = selectedRow[0][0];
            if (CommonUtils.IsValid(stockCode)) {
                Notifications.ShowOverlayConfirmation(
                    'Delete Confirmation',
                    'Do you want to delete the current selected item. StockCode: ' + stockCode + ' ?',
                    StockHandler.DeleteRecordFunction,
                    {'stockCode': stockCode});
            }
        }
        else {
            Notifications.ShowOverlayWarningNotification('Select one row', 'Please select one row to delete');
        }
    }
}

StockHandler.DeleteRecordFunction = function (obj) {
    if (CommonUtils.IsNotUndefinedAndNull(obj)) {
        CommonUtils.BlurLoadingScreen();
        setTimeout(function () { StockHandler.RemoveUserStockCode(obj.stockCode); }, 2000);
    }
}

StockHandler.RemoveUserStockCode = function (stockCode) {
    var token = CommonUtils.GetItemFromLocalStorage(UserLoginHandler.DefaultTokenStorageKey);
    if (CommonUtils.IsValid(token)) {
        CommonUtils.BlurLoadingScreen();
        $.ajax(
            'http://localhost/CSharpAssignment/StockExchangeWS.asmx/RemoveUserStockItem',
            {
                cache: true,
                async: true,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ token: token, stockCode: stockCode }),
                success: function (json) { StockHandler.ProcessRemoveUserStockCode(json); },
                error: function (error) { StockHandler.FailureStockUserList(error); }
            });
    }
}

StockHandler.ProcessRemoveUserStockCode = function (json) {
    var jsonObj = JSON.parse(json.d);
    if (CommonUtils.IsNotUndefinedAndNull(jsonObj)) {
        if (CommonUtils.IsValid(jsonObj.Messages)) {
            CommonUtils.RemoveLoadingScreen();
            CommonUtils.EnableHtmlComponentByID('delete-record-grid-btn', false);
            MessageProcessor.ProcessMessages(jsonObj.Messages);
        }
        else {
            StockHandler.RefreshUserGridData();
            CommonUtils.EnableHtmlComponentByID('delete-record-grid-btn', false);
            $('#myModal').modal('hide');
            CommonUtils.RemoveLoadingScreen();
        }
    }
}

StockHandler.GetStockGridTable = function () {
    return $('#stock-list-grid').DataTable();
}

StockHandler.GetAllStockGridTable = function () {
    var grid = $('#all-stock-list-grid');
    if (CommonUtils.IsValid(grid)) {
        if (grid.attr('role') === 'grid') {
            return grid.DataTable();
        }
    }
    return undefined;
}

StockHandler.DestroyTable = function (tableID) {
    var grid = $('#' + tableID);
    if (CommonUtils.IsValid(grid)) {
        grid.dataTable().fnDestroy();
        grid.empty();
        grid.removeAttr('role');
        grid.removeAttr('aria-decribedby');
        grid.removeClass('dataTable');
        grid.removeClass('no-footer');
    }
    return undefined;
}