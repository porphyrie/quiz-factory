import React, { useState } from 'react'
import { Accordion, Button, Container, Row, Stack } from 'react-bootstrap';
import Navigation from '../components/Navigation'

export default function TestResults() {

  const [testName, setTestName] = useState('Structuri de date și algoritmi');

  const [name, setName] = useState({
    lastName: "Cristinel",
    firstName: "Maria"
  });

  const [grade, setGrade] = useState(5);

  const [results, setResults] = useState([
    {
      "question": "Fie vf un pointer către vârful unei stive {5 7 1 8 2} . Care va fi conținutul stivei în urma execuției funcției generate_output()?",
      "correctAnswer": "14 18 6 20 2",
      "studentAnswer": "14 18 6 20 2"
    },
    {
      "question": "Se consideră următoarea listă dublu înlănțuită {2 8 3 5} . Ce va afișa funcția generate_output()?",
      "correctAnswer": "6 8 20 5 5",
      "studentAnswer": "6 7 19 5 5"
    }
  ]);

  const string = `void generate_output() {
    nod *q = vf;
    while (q->prec) {
        q->n += 2;
        if (q->n != 5)
            q->n *= 2;
        q = q->prec;
    }
}
`;

  return (
    <Container className='bg-violet-300 p-10 space-y-5'>
      <Row>
        <h4 className='font-bold px-0 text-center'>{testName}</h4>
      </Row>
      <Row>
        <h6 className='font-bold'>Data: 10/3/2018 22:20</h6>
        <h6 className='font-bold'>Durata: 90 min</h6>
        <h6 className='font-bold'>Nr. itemi: x</h6>
      </Row>
      <Row>
        <Stack direction="horizontal">
          <h6 className='font-bold px-0'>{name.lastName} {name.firstName}</h6>
          <h6 className='font-bold px-0 ms-auto'>NOTA: {grade}</h6>
        </Stack>
      </Row>
      <Row>
        <Accordion flush alwaysOpen>{
          results.map((r, i) => (
            <Accordion.Item eventKey={i}>
              <Accordion.Header>{i + 1}. {r.question}</Accordion.Header>
              <Accordion.Body>
                <h6>{r.question}</h6>
                <pre>{string}</pre>
                <h6>C: {r.correctAnswer}</h6>
                {r.correctAnswer === r.studentAnswer
                  ? <p className='text-green-600'>R: {r.studentAnswer}</p>
                  : <p className='text-red-600'>R: {r.studentAnswer}</p>
                }
              </Accordion.Body>
            </Accordion.Item>
          ))}
        </Accordion>
      </Row>
    </Container >
  )
}
