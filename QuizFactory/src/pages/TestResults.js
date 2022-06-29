import React, { useEffect, useState } from 'react'
import { Accordion, Button, Container, Row, Stack } from 'react-bootstrap';
import { useSearchParams, useParams } from 'react-router-dom';
import Navigation from '../components/Navigation'
import { createAPIEndpoint, ENDPOINTS } from '../helpers/API';
import { formatDate, getCurrDate } from '../helpers/Date';

export default function TestResults() {

  const params = useSearchParams()[0];

  const [testResults, setTestResults] = useState({});

  useEffect(() => {
    console.log(params);
    createAPIEndpoint(ENDPOINTS.results)
      .authFetchByParams(params)
      .then(res => {
        setTestResults(res.data);
        console.log(res.data);
        console.log(getCurrDate().toLocaleString());
      })
      .catch(err => alert(err));
  }, []);

  return (
    <Container className='bg-violet-300 p-10 space-y-5'>
      {Object.keys(testResults).length
        ? <>
          <Row>
            <h2 className='font-bold px-0 text-center'>{testResults.testName}</h2>
          </Row>
          <Row>
            <h6 className='font-bold'>Data: {formatDate(testResults.testDate)}</h6>
            <h6 className='font-bold'>Durata: {testResults.testDuration} min</h6>
            <h6 className='font-bold'>Nr. itemi: {testResults.itemCount}</h6>
          </Row>
          <Row>
            <Stack direction="horizontal">
              <h6 className='font-bold px-0'>{testResults.lastName} {testResults.firstName}</h6>
              <h6 className='font-bold px-0 ms-auto'>Punctaj: {testResults.grade}</h6>
            </Stack>
          </Row>
          <Row>
            <Accordion flush alwaysOpen>{
              testResults.answers.map((r, i) => (
                <Accordion.Item eventKey={i}>
                  <Accordion.Header>{i + 1}. {r.question}</Accordion.Header>
                  <Accordion.Body>
                    <pre className='whitespace-pre-wrap'>{r.question}</pre>
                    <h6>C: {r.correctAnswer}</h6>
                    {r.correctAnswer === r.answer
                      ? <p className='text-green-600'>R: {r.answer === '' ? 'gol' : r.answer}</p>
                      : <p className='text-red-600'>R: {r.answer === '' ? 'gol' : r.answer}</p>
                    }
                  </Accordion.Body>
                </Accordion.Item>
              ))}
            </Accordion>
          </Row>
        </>
        : <></>
      }
    </Container >
  )
}
