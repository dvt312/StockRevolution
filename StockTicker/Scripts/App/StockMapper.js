// Copyright(c) Daniel Veintimilla 2016.

function StockMapper() { }

StockMapper.MapStockListToStockGridData = function (stockList) {
    var array = new Array();
    if (CommonUtils.IsValid(stockList)) {
        for (i = 0; i < stockList.length; i++) {
            var stock = stockList[i];
            array.push(new Array(stock.StockCode, stock.StockName, stock.Price));
        }
    }

    return array;
}