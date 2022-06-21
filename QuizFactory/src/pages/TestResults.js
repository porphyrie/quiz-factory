import React, { useEffect, useState } from 'react'
import { Accordion, Button, Container, Row, Stack } from 'react-bootstrap';
import { useSearchParams } from 'react-router-dom';
import Navigation from '../components/Navigation'
import { createAPIEndpoint, ENDPOINTS } from '../helpers/API';

export default function TestResults() {

  const params = {
    username: useSearchParams().get('username'),
    testId: useSearchParams().get('testId')
  }

  const [testResults, setTestResults] = useState({});

  useEffect(() => {
    createAPIEndpoint(ENDPOINTS.tests)
      .authFetchByParams(params)
      .then(res => {
        setTestResults(res.data);
        console.log(res.data);
      })
      .catch(err => alert(err));
  }, []);

  const formatDate = (date) => {
    let tempdate = new Date(date);
    let day = tempdate.getDate();
    let month = tempdate.getMonth();
    let year = tempdate.getFullYear();
    let hour = tempdate.getHours();
    let min = tempdate.getMinutes();

    return day + '/' + month + '/' + year + " " + hour + ":" + min;
  }


  //   const [testName, setTestName] = useState('Structuri de date și algoritmi');

  //   const [name, setName] = useState({
  //     lastName: "Cristinel",
  //     firstName: "Maria"
  //   });

  //   const [grade, setGrade] = useState(5);

  //   const [results, setResults] = useState([
  //     {
  //       "question": "Fie vf un pointer către vârful unei stive {5 7 1 8 2} . Care va fi conținutul stivei în urma execuției funcției generate_output()?",
  //       "correctAnswer": "14 18 6 20 2",
  //       "studentAnswer": "14 18 6 20 2"
  //     },
  //     {
  //       "question": "Se consideră următoarea listă dublu înlănțuită {2 8 3 5} . Ce va afișa funcția generate_output()?",
  //       "correctAnswer": "6 8 20 5 5",
  //       "studentAnswer": "6 7 19 5 5"
  //     }
  //   ]);

  //   const string = `void generate_output() {
  //     nod *q = vf;
  //     while (q->prec) {
  //         q->n += 2;
  //         if (q->n != 5)
  //             q->n *= 2;
  //         q = q->prec;
  //     }
  // }
  // `;

  return (
    <Container className='bg-violet-300 p-10 space-y-5'>
      <Row>
        <h4 className='font-bold px-0 text-center'>{testResults.testName}</h4>
      </Row>
      <Row>
        <h6 className='font-bold'>Data: {formatDate(testResults.testDate)}</h6>
        <h6 className='font-bold'>Durata: {testResults.testDuration} min</h6>
        <h6 className='font-bold'>Nr. itemi: {testResults.itemCount}</h6>
      </Row>
      <Row>
        <Stack direction="horizontal">
          <h6 className='font-bold px-0'>{testResults.lastName} {testResults.firstName}</h6>
          <h6 className='font-bold px-0 ms-auto'>NOTA: {testResults.grade}</h6>
        </Stack>
      </Row>
      <Row>
        <Accordion flush alwaysOpen>{
          testResults.answers.map((r, i) => (
            <Accordion.Item eventKey={i}>
              <Accordion.Header>{i + 1}. {r.question}</Accordion.Header>
              <Accordion.Body>
                <h6>{r.question}</h6>
                <pre>{r.question}</pre>
                <h6>C: {r.correctAnswer}</h6>
                {r.correctAnswer === r.answer
                  ? <p className='text-green-600'>R: {r.answer}</p>
                  : <p className='text-red-600'>R: {r.answer}</p>
                }
              </Accordion.Body>
            </Accordion.Item>
          ))}
        </Accordion>
      </Row>
    </Container >
  )
}
