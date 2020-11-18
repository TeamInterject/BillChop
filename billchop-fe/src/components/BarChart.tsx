import * as React from "react";
import { Col, Row } from "react-bootstrap";
import { Bar, ChartData } from "react-chartjs-2";

export interface IBarChartDataset<T> {
  label: string;
  data: T;
}

export interface IBarChartProps<T> {
  datasets: IBarChartDataset<T>[];
  headingText?: string;
}

export default class BarChart<T> extends React.Component<IBarChartProps<T>> {
  getChartData = (): ChartData<Chart.ChartData> => {
    const { datasets } = this.props;

    return {
      labels: datasets.map((set) => set.label),
      datasets: [
        {
          backgroundColor: "#2980b9",
          borderColor: "#2c3e50",
          borderWidth: 1,
          hoverBackgroundColor: "#3498db",
          hoverBorderColor: "#34495e",
          barPercentage: 0.3,
          data: datasets.map((set) => set.data),
        },
      ],
    };
  };

  getChartOptions = (): Chart.ChartOptions => {
    return {
      scales: {
        yAxes: [
          {
            ticks: {
              min: 0,
            },
          },
        ],
      },
    };
  };

  render(): JSX.Element {
    const { headingText } = this.props;

    return (
      <div className="m-2">
        <Row>
          <Col className="text-center">
            {{ headingText } && <h5>{headingText}</h5>}
          </Col>
        </Row>
        <Row>
          <Col>
            <Bar
              data={this.getChartData()}
              legend={{ display: false }}
              options={this.getChartOptions()}
            />
          </Col>
        </Row>
      </div>
    );
  }
}
