import React from 'react'
import { useState, useEffect, props } from 'react';
import { Button, Col, Container, FormControl, InputGroup, Row, Stack } from 'react-bootstrap';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { useTimer } from 'react-timer-hook';
import Navigation from '../../components/Navigation'
import { createAPIEndpoint, ENDPOINTS } from '../../helpers/API';
import { addMinToDate, getCurrDate } from '../../helpers/Date';
import { getUsername } from '../../helpers/User';
import TestResults from '../TestResults';

export default function Question() {

  const params = {
    username: useSearchParams()[0].get('username'),
    testId: useSearchParams()[0].get('testId')
  }

  let expiryTimestamp = new Date();
  expiryTimestamp = expiryTimestamp.setSeconds(expiryTimestamp.getSeconds() + 600)

  const [testSummary, setTestSummary] = useState({});

  const {
    seconds,
    minutes,
    hours,
    restart
  } = useTimer({
    expiryTimestamp, onExpire: () => {
      alert('Timpul a expirat!');
      handleFinish();
    }
  });

  useEffect(() => {
    createAPIEndpoint(ENDPOINTS.test)
      .authFetchById(params.testId)
      .then(res => {
        setTestSummary(res.data);
        const time = addMinToDate(res.data.testDate, res.data.testDuration);
        restart(time);
        //sa readuca din localstorage ce e de adus, sa puna in cele 3 state uri
      })
      .catch(err => alert(err));
  }, []);

  const [genQuestion, setGenQuestion] = useState(false);

  const [resultId, setResultId] = useState('');

  useEffect(() => {
    if (Object.keys(testSummary).length) {
      const fields = {
        testId: testSummary.testId,
        studentUsername: getUsername()
      }
      createAPIEndpoint(ENDPOINTS.results)
        .authPost(fields)
        .then(res => {
          setResultId(res.data.resultId);
          console.log(res.data);
          console.log(res.data.resultId);
          setGenQuestion(true);
        })
        .catch(err => alert(err));
    }
  }, [testSummary]);

  const [counter, setCounter] = useState(0);

  const [resultDetailsId, setResultDetailsId] = useState(0);

  const [question, setQuestion] = useState('');

  useEffect(() => {
    if (genQuestion === true) {
      const params = {
        testId: testSummary.testId,
        username: getUsername(),
        questionTypeId: testSummary.questionTypes[counter].id
      }
      createAPIEndpoint(ENDPOINTS.generatequestions)
        .authFetchByParams(params)
        .then(res => {
          setResultDetailsId(res.data.resultId);
          setQuestion(res.data.question);
        })
        .catch(err => alert(err));
    }
  }, [genQuestion, counter]);

  const [response, setResponse] = useState("");

  const [textareaPlaceholder, setTextareaPlaceholder] = useState('Introdu aici răspunsul tău')

  const handleAnswerChange = e => {
    setResponse(e.target.value);
  }

  const handleNextQuestion = () => {
    const patchDocument = [{
      op: 'replace',
      path: '/answer',
      value: response,
    }];
    console.log(resultDetailsId);
    console.log(patchDocument);
    createAPIEndpoint(ENDPOINTS.resultdetails)
      .authPatch(resultDetailsId, patchDocument)
      .then(res => {
        if (counter < testSummary.questionTypes.length - 1)
          setCounter(counter + 1);
        setTextareaPlaceholder('Introdu aici răspunsul tău');
      })
      .catch(err => alert(err));
  }

  const navigate = useNavigate();

  const handleFinish = () => {
    handleNextQuestion();
    const patchDocument = [{
      op: 'replace',
      path: '/finishtime',
      value: getCurrDate().toLocaleString()
    }];
    createAPIEndpoint(ENDPOINTS.results)
      .authPatch(resultId, patchDocument)
      .then(res => {
        alert('Răspunsurile tale au fost trimise!');
        navigate("/tests");
      })
      .catch(err => alert(err));
    //sterge din local storage obiectul
  }

  return (
    <Container className='bg-violet-300 p-10 space-y-5'>
      {Object.keys(testSummary).length > 0 ?
        <>
          <Row className='pb-10'>
            <h2 className='font-bold text-center'>{testSummary.testName}</h2>
          </Row>
          <Row>
            {question === ''
              ? <pre className='font-bold text-xl whitespace-pre-wrap'>{counter + 1}. {testSummary.questionTypes[counter].templateString}</pre>
              : <pre className='font-bold text-xl whitespace-pre-wrap'>{counter + 1}. {question}</pre>}
          </Row>
          <Row>
            <FormControl as="textarea" onChange={handleAnswerChange} placeholder={textareaPlaceholder} />
          </Row>
          <Row>
            <Stack direction="horizontal" className='px-0'>
              <h6 className='font-bold'>{counter + 1} / {testSummary.itemCount} itemi | {hours}:{minutes}:{seconds}</h6>
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