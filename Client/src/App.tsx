import React, {useEffect, useState} from 'react';
import {
  LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, TooltipProps
} from 'recharts';

import styled from 'styled-components'

type AggregatedOffer = {
    averageArea: number;
    areaUnit: string;
    priceUnit: string;
    averagePricePerUnit: string;
    type: string;
    city: string;
    averagePrice: string;
    createdOn: string;
    count: number;
}

const AppContainer = styled.div`
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: center;
  align-items:center;

`;

const ChartContainer = styled.div`
  max-width: 830px;
  max-height: 440px;
`;

const TooltipContainer = styled.div`
  border: 1px solid grey;
  padding: 10px;
`;

function App() {

  const [aggregatedOffers, setAggregatedOffers] = useState<AggregatedOffer[]>([]);

  useEffect(() => {
    async function fetchData(){
      const response = await fetch("/api/offers/aggregated");
      const data = await response.json();
      setAggregatedOffers(data);
    }

    fetchData();
  }, [])

  const wroclawOffers = aggregatedOffers.filter(o => o.city === "Wroclaw").sort((o1, o2) =>  Date.parse(o1.createdOn) - Date.parse(o2.createdOn));
  const warsawOffers = aggregatedOffers.filter(o => o.city === "Warsaw").sort((o1, o2) =>  Date.parse(o1.createdOn) - Date.parse(o2.createdOn));

  return (
    <AppContainer>
      {wroclawOffers.length > 0
      ? 
      <ChartContainer><h2>Wroclaw offers: m2 average price per day</h2><Chart data={wroclawOffers} /></ChartContainer>
      : <p>No offers for Wroclaw</p>}
      {warsawOffers.length > 0
      ?<ChartContainer><h2>Warsaw offers: m2 average price per day</h2><Chart data={warsawOffers} /></ChartContainer>
      : <p>No offers for Warsaw</p>}
    </AppContainer>
  );
}

type ChartProps = {
  data: AggregatedOffer[];
}

function Chart({data} : ChartProps) {
  return (
    <LineChart
      width={800}
      height={400}
      data={data}
      margin={{
        top: 5, right: 30, left: 20, bottom: 5,
      }}
    >
      <CartesianGrid strokeDasharray="3 3" />
      <XAxis dataKey="createdOn" />
      <YAxis />
      <Tooltip content={<CustomTooltip />}/>
      <Legend />
      <Line type="monotone" dataKey="averagePricePerUnit" stroke="#82ca9d" />
    </LineChart>
  );
}

function CustomTooltip ({ active, payload, label }: TooltipProps) {
  if (active) {
    return (
      <TooltipContainer>
        <h4>Values for date: {label}.</h4>
        <p>{`Average price: ${payload![0].value} PLN / M2`}</p>
        <p>Offers posted: {payload![0].payload.count}.</p>
      </TooltipContainer>
    );
  }

  return null;
};

export default App;
