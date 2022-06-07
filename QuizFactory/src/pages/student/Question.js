import React from 'react'
import { useState, useEffect, props } from 'react';
import { Button, Col, Container, FormControl, InputGroup, Row, Stack } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import Navigation from '../../components/Navigation'
import TestResults from '../TestResults';

export default function Question() {

  const startminutes = 10;
  const time = startminutes * 60;

  function updateCount() {
    const minutes = Math.floor(time / 60);
    const seconds = time % 60;
    time--;
  }

  setInterval((updateCount, 1000));

  const [test, setTest] = useState([{
    id: 0,
    intrebare: "Fie vf un pointer către vârful unei stive {5 7 1 8 2} . Care va fi conținutul stivei în urma execuției funcției de mai jos?",
  }, {
    id: 1,
    intrebare: "Se consideră următoarea listă dublu înlănțuită ... . Ce va afișa funcția ...?",
  }, {
    id: 2,
    intrebare: "Care este sortarea topologica a grafului directionat ...?",
  }, {
    id: 3,
    intrebare: "Care este parcurgerea DFS a grafului directionat ... incepand din nodul 0?",
  }]);

  const [counter, setCounter] = useState(0);

  const [minutes, setMinutes] = useState(60);
  const [seconds, setSeconds] = useState(0);

  useEffect(() => {
    let myInterval = setInterval(() => {
      if (seconds > 0) {
        setSeconds(seconds - 1);
      }
      if (seconds === 0) {
        if (minutes === 0) {
          clearInterval(myInterval)
        } else {
          setMinutes(minutes - 1);
          setSeconds(59);
        }
      }
    }, 1000)
    return () => {
      clearInterval(myInterval);
    };
  });

  const navigate = useNavigate();

  const handleFinish = () => {
    alert('Răspunsurile tale au fost trimise!')
    navigate("/tests");
  }

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
        <h5 className='font-bold'>{counter + 1}. {test[counter].intrebare}</h5>
        <pre className='font-bold text-xl'>{string}</pre>
      </Row>
      <Row>
        <FormControl as="textarea" placeholder='Introdu aici răspunsul tău' />
      </Row>
      <Row>
        <Stack direction="horizontal" className='px-0'>
          <h6 className='font-bold'>{counter + 1} / {test.length} itemi | {minutes}:{seconds} minute rămase</h6>
          {counter < test.length - 1
            ? <Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white ms-auto' onClick={() => setCounter(counter + 1)}>Următoarea întrebare</Button>
            : <Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white ms-auto' onClick={handleFinish}>Termină testul</Button>
          }
        </Stack>
      </Row>
    </Container>
  )
}