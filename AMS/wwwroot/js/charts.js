/* eslint-disable object-curly-newline */

/* global Chart */

/**
 * --------------------------------------------------------------------------
 * CoreUI Free Boostrap Admin Template (v2.0.0): main.js
 * Licensed under MIT (https://coreui.io/license)
 * --------------------------------------------------------------------------
 */

/* eslint-disable no-magic-numbers */
// random Numbers
var random = function random() {
  return Math.round(Math.random() * 100);
}; // eslint-disable-next-line no-unused-vars

var chartMonthlyTicketsModel;
var chartTicketsByLocationModel;
var chartTicketsStatusModel;
var chartTasksStatusModel;
var lineChart;
var barChart;
var doughnutChart;
var pieChart;

$.getJSON('/Home/GetMonthlyTicketsData', {}, function (data) {
    chartMonthlyTicketsModel = data;
    /*
    $.each(data, function (index, element) {
        $('body').append($('<div>', {
            text: element.name
        }));
    });
    */
    console.log(chartMonthlyTicketsModel);
    lineChart = new Chart($('#canvas-1'), {
        type: 'line',
        data: {
            labels: chartMonthlyTicketsModel.labels,
            datasets: [{
                label: chartMonthlyTicketsModel.dataSetLabels[0].label,
                backgroundColor: 'rgba(220, 220, 220, 0.2)',
                borderColor: 'rgba(220, 220, 220, 1)',
                pointBackgroundColor: 'rgba(220, 220, 220, 1)',
                pointBorderColor: '#fff',
                data: chartMonthlyTicketsModel.dataSetLabels[0].data
            }, {
                label: chartMonthlyTicketsModel.dataSetLabels[1].label,
                backgroundColor: 'rgba(151, 187, 205, 0.2)',
                borderColor: 'rgba(151, 187, 205, 1)',
                pointBackgroundColor: 'rgba(151, 187, 205, 1)',
                pointBorderColor: '#fff',
                data: chartMonthlyTicketsModel.dataSetLabels[1].data
            }]
        },
        options: {
            responsive: true
        }
    }); // eslint-disable-next-line no-unused-vars

});

$.getJSON('/Home/GetLocationTicketsData', {}, function (data) {
    chartTicketsByLocationModel = data;
    
    barChart = new Chart($('#canvas-2'), {
        type: 'bar',
        data: {
            labels: chartTicketsByLocationModel.labels,
            datasets: [{
                label: chartMonthlyTicketsModel.dataSetLabels[0].label,
                backgroundColor: 'rgba(220, 220, 220, 0.5)',
                borderColor: 'rgba(220, 220, 220, 0.8)',
                highlightFill: 'rgba(220, 220, 220, 0.75)',
                highlightStroke: 'rgba(220, 220, 220, 1)',
                data: chartTicketsByLocationModel.dataSetLabels[0].data
            }, {
                label: chartTicketsByLocationModel.dataSetLabels[1].label,
                backgroundColor: 'rgba(151, 187, 205, 0.5)',
                borderColor: 'rgba(151, 187, 205, 0.8)',
                highlightFill: 'rgba(151, 187, 205, 0.75)',
                highlightStroke: 'rgba(151, 187, 205, 1)',
                data: chartTicketsByLocationModel.dataSetLabels[1].data
            }]
        },
        options: {
            responsive: true
        }
    }); // eslint-disable-next-line no-unused-vars

});


$.getJSON('/Home/GetTicketsStatusData', {}, function (data) {
    chartTasksStatusModel = data;

    // Tickets Status
    doughnutChart = new Chart($('#canvas-3'), {
        type: 'doughnut',
        data: {
            labels: chartTasksStatusModel.labels,
            datasets: [{
                data: chartTasksStatusModel.dataSetLabels[0].data,
                backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56'],
                hoverBackgroundColor: ['#FF6384', '#36A2EB', '#FFCE56']
            }]
        },
        options: {
            responsive: true
        }
    }); // eslint-disable-next-line no-unused-vars
});

$.getJSON('/Home/GetTasksStatusData', {}, function (data) {
    chartTicketsStatusModel = data;

    // Tasks Status
    pieChart = new Chart($('#canvas-5'), {
        type: 'pie',
        data: {
            labels: ['Red', 'Green', 'Yellow'],
            datasets: [{
                data: [300, 50, 100],
                backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56'],
                hoverBackgroundColor: ['#FF6384', '#36A2EB', '#FFCE56']
            }]
        },
        options: {
            responsive: true
        }
    }); // eslint-disable-next-line no-unused-vars
    doughnutChart = new Chart($('#canvas-5'), {
        type: 'doughnut',
        data: {
            labels: chartTicketsStatusModel.labels,
            datasets: [{
                data: chartTicketsStatusModel.dataSetLabels[0].data,
                backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56'],
                hoverBackgroundColor: ['#FF6384', '#36A2EB', '#FFCE56']
            }]
        },
        options: {
            responsive: true
        }
    }); // eslint-disable-next-line no-unused-vars
});






var radarChart = new Chart($('#canvas-4'), {
  type: 'radar',
  data: {
    labels: ['Eating', 'Drinking', 'Sleeping', 'Designing', 'Coding', 'Cycling', 'Running'],
    datasets: [{
      label: 'My First dataset',
      backgroundColor: 'rgba(220, 220, 220, 0.2)',
      borderColor: 'rgba(220, 220, 220, 1)',
      pointBackgroundColor: 'rgba(220, 220, 220, 1)',
      pointBorderColor: '#fff',
      pointHighlightFill: '#fff',
      pointHighlightStroke: 'rgba(220, 220, 220, 1)',
      data: [65, 59, 90, 81, 56, 55, 40]
    }, {
      label: 'My Second dataset',
      backgroundColor: 'rgba(151, 187, 205, 0.2)',
      borderColor: 'rgba(151, 187, 205, 1)',
      pointBackgroundColor: 'rgba(151, 187, 205, 1)',
      pointBorderColor: '#fff',
      pointHighlightFill: '#fff',
      pointHighlightStroke: 'rgba(151, 187, 205, 1)',
      data: [28, 48, 40, 19, 96, 27, 100]
    }]
  },
  options: {
    responsive: true
  }
}); // eslint-disable-next-line no-unused-vars



var polarAreaChart = new Chart($('#canvas-6'), {
  type: 'polarArea',
  data: {
    labels: ['Red', 'Green', 'Yellow', 'Grey', 'Blue'],
    datasets: [{
      data: [11, 16, 7, 3, 14],
      backgroundColor: ['#FF6384', '#4BC0C0', '#FFCE56', '#E7E9ED', '#36A2EB']
    }]
  },
  options: {
    responsive: true
  }
});
//# sourceMappingURL=charts.js.map