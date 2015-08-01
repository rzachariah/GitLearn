// A simple templating method for replacing placeholders enclosed in curly braces.
if (!String.prototype.supplant) {
    String.prototype.supplant = function (o) {
        return this.replace(/{([^{}]*)}/g,
            function (a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            }
        );
    };
}

$(function () {
     
     var ticker = $.connection.viewHub,
        $stockTable = $('#stockTable'),
        $stockTableBody = $stockTable.find('tbody'),
        rowTemplate = '<tr data-Id="{Id}"><td>{Id}</td><td>{Amount}</td><td>{Price}</td><td>{MarketValue}</td></tr>';

    function init() {
        ticker.server.getAllStocks().done(function (stocks) {
            $stockTableBody.empty();
        });
    }

    // Add a client-side hub method that the server will call
    ticker.client.updateStockPrice = function (stocks) {
        $.each(stocks, function () {
			$row = $(rowTemplate.supplant(this));

			var row = $stockTableBody.find('tr[data-Id=' + this.Id + ']');
			if (row.length == 0)
				$stockTableBody.append(rowTemplate.supplant(this));
			else
				row.replaceWith($row);
		});
        }

    // Start the connection
    $.connection.hub.logging = true;
    $.connection.hub.start({ transport: activeTransport, jsonp: isJsonp }).done(init);
});