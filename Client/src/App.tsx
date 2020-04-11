import React, {useEffect, useState, useCallback} from 'react';
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
  margin-top: 50px;
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: center;
  align-items:center;
  padding: 30px 40px;
`;

const FormContainer = styled.div`
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  background-color: grey;
  padding: 10px 20px;
  text-align:center;

  & input {
    padding: 5px 10px;
    margin: 0 10px;
    width: 100px;
  }
`;

const FlexItemContainer = styled.div`
  max-width: 830px;
  max-height: 440px;
`;

const TooltipContainer = styled.div`
  border: 1px solid grey;
  padding: 10px;
`;

async function getData(minSize: number, maxSize: number): Promise<AggregatedOffer[]> {
  const response = await fetch(`/api/offers/aggregated?MinSize=${minSize}&MaxSize=${maxSize}`);
  return await response.json();
}

function App() {

  const [aggregatedOffers, setAggregatedOffers] = useState<AggregatedOffer[]>([]);
  const [minArea, setMinArea] = useState<number>(0);
  const [maxArea, setMaxArea] = useState<number>(1000);


  useEffect(() => {
    async function fetchData(){
      const data =  await getData(minArea,maxArea);
      setAggregatedOffers(data);
    }

    fetchData();
  }, [])

  const getOffers = useCallback(
    async () => {
      const data =  await getData(minArea, maxArea);
      setAggregatedOffers(data);
    },
    [minArea, maxArea],
  );

  const wroclawOffers = aggregatedOffers.filter(o => o.city === "Wroclaw").sort((o1, o2) =>  Date.parse(o1.createdOn) - Date.parse(o2.createdOn));
  const warsawOffers = aggregatedOffers.filter(o => o.city === "Warsaw").sort((o1, o2) =>  Date.parse(o1.createdOn) - Date.parse(o2.createdOn));

  return (
    <>
    <FormContainer>
        <label>Min area:</label><input value={minArea} onChange={event => setMinArea(Number(event.target.value))} />
        <label>Max area:</label><input value={maxArea} onChange={event => setMaxArea(Number(event.target.value))} />
        <input onClick={getOffers} type="button" value="Get offers"></input>
      </FormContainer>
    <AppContainer>
      {wroclawOffers.length > 0
      ? 
      <FlexItemContainer><h2>Wroclaw offers: m2 average price per day</h2><Chart data={wroclawOffers} /></FlexItemContainer>
      : <p>No offers for Wroclaw</p>}
      {warsawOffers.length > 0
      ?<FlexItemContainer><h2>Warsaw offers: m2 average price per day</h2><Chart data={warsawOffers} /></FlexItemContainer>
      : <p>No offers for Warsaw</p>}
    </AppContainer>
    </>
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
