import React from 'react'
import { useState, useEffect, props } from 'react';
import { Button, Col, Container, FormControl, InputGroup, Row, Stack } from 'react-bootstrap';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { useTimer } from 'react-timer-hook';
import Navigation from '../../components/Navigation'
import { createAPIEndpoint, ENDPOINTS } from '../../helpers/API';
import TestResults from '../TestResults';

export default function Question() {

  const params = {
    username: useSearchParams()[0].get('username'),
    testId: useSearchParams()[0].get('testId')
  }

  let expiryTimestamp = new Date();
  expiryTimestamp = expiryTimestamp.setSeconds(expiryTimestamp.getSeconds() + 600)

  const addMinToDate = (date, min) => {
    date = new Date(date);
    return new Date(date.getTime() + min * 60000);
  }

  const [testSummary, setTestSummary] = useState({});

  const {
    seconds,
    minutes,
    restart
  } = useTimer({
    expiryTimestamp, onExpire: () => {
      alert('Timpul a expirat!');
      handleFinish();
    }
  });

  const [counter, setCounter] = useState(0);

  const [questionId, setQuestionId] = useState(0);

  const [question, setQuestion] = useState('');

  useEffect(() => {
    createAPIEndpoint(ENDPOINTS.test)
      .authFetchById(params.testId)
      .then(res => {
        setTestSummary(res.data);
        const time = addMinToDate(res.data.testDate, res.data.testDuration * 1000);
        restart(time);
        //sa readuca din localstorage ce e de adus, sa puna in cele 3 state uri
      })
      .catch(err => alert(err));
  }, []);

  const navigate = useNavigate();

  //un useeffect care se modifica la counter
  //daca testsummary este setat
  //face request pentru urmatoarea intrebare din questiontype[counter] si seteaza intrebarea si id-ul

  const handleNextQuestion = () => {
    setCounter(counter + 1);
    //cand dau pe urm intrebare
    //trimit la id-ul ala raspunsul dat
  }

  const handleFinish = () => {
    alert('Răspunsurile tale au fost trimise!')
    navigate("/tests");
    //sterge din local storage obiectul
    //de trimis datacurenta ca finish time
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
      {Object.keys(testSummary).length > 0 ?
        <>
          <Row className='pb-10'>
            <h2 className='font-bold text-center'>{testSummary.testName}</h2>
          </Row>
          <Row>
            <h5 className='font-bold'>{counter + 1}. {testSummary.questionTypes[counter].templateString}</h5>
            <pre className='font-bold text-xl'>{string}</pre>
          </Row>
          <Row>
            <FormControl as="textarea" placeholder='Introdu aici răspunsul tău' />
          </Row>
          <Row>
            <Stack direction="horizontal" className='px-0'>
              <h6 className='font-bold'>{counter + 1} / {testSummary.itemCount} itemi | {minutes}:{seconds} minute rămase</h6>
              {counter < testSummary.itemCount - 1
                ? <Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white ms-auto' onClick={handleNextQuestion}>Următoarea întrebare</Button>
                : <Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white ms-auto' onClick={handleFinish}>Termină testul</Button>
              }
            </Stack>
          </Row>
        </>
        : <></>}
    </Container>
  )
}