import React, { useEffect } from 'react'
import Navigation from '../components/Navigation'
import { useState } from 'react'
import { parse } from 'postcss'
import { Link, useNavigate } from 'react-router-dom'
import { Button, Container, Row, Table } from 'react-bootstrap'
import { createAPIEndpoint, ENDPOINTS } from '../helpers/API'

export default function Tests() {

  const getUsername = () => {
    const userData = JSON.parse(localStorage.getItem('user'));
    if (userData === null)
      return '';
    return userData.username;
  }

  const [tests, setTests] = useState([]);

  useEffect(() => {
    createAPIEndpoint(ENDPOINTS.tests)
      .authFetchById(getUsername())
      .then(res => {
        setTests(res.data);
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

  const getCurrDate = () => {
    return new Date();
  }

  const addMinToDate = (date, min) => {
    return new Date(date.getTime() + min * 60000);
  }

  const getUserType = () => {
    const userData = JSON.parse(localStorage.getItem('user'));
    if (userData === null)
      return '';
    return userData.role;
  }

  const navigate = useNavigate();

  const handleBeginTest = (testId) => {
    navigate('/question/?username=' + getUsername() + '&testId=' + testId);
  }

  const handleVisualizeResults = (testId) => {
    navigate('/testresults/?username=' + getUsername() + '&testId=' + testId);
  }

  const handleVisualizeDetails = (testId) => {
    navigate("/test/" + testId);
  }

  return (
    <Container className='bg-violet-300 p-10 space-y-5'>
      <Row className='pb-10'>
        <h2 className='font-bold text-center'>Teste</h2>
      </Row>
      {getUserType() === 'profesor'
        ?
        <Row>
          <h4 className='font-bold flex-none'><a href='/addquestions' className='text-violet-600 hover:text-violet-900'>Creează un test</a></h4>
        </Row>
        : <></>
      }
      <Row>
        <h4 className='font-bold flex-none'>Vizualizează testele </h4>
        <Table responsive striped bordered hover className='bg-white'>
          <thead>
            <tr>
              <th>Curs</th>
              <th>Denumire</th>
              <th>Dată</th>
              <th>Durată</th>
              <th>Nr. itemi</th>
            </tr>
          </thead>
          <tbody>
            {
              tests.length
                ? <>
                  {
                    tests.map((test) => (
                      <tr>
                        <td>{test.courseName}</td>
                        <td>{test.testName}</td>
                        <td>{formatDate(test.testDate)}</td>
                        <td>{test.testDuration} min</td>
                        <td>{test.itemCount}</td>
                        <td>
                          <Container className='space-y-1'>
                            {getUserType() === 'profesor'
                              ? (<Row><Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={() => handleVisualizeDetails(test.testId)}>Detalii</Button></Row>)
                              : (
                                getCurrDate() < test.testDate
                                  ?
                                  <>
                                    <Row><Button type='button' disabled className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={() => handleBeginTest(test.testId)}>Începe testul</Button></Row>
                                    <Row><Button type='button' disabled className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={handleVisualizeResults(test.testId)}>Vizualizează rezultatele</Button></Row>
                                  </>
                                  :
                                  getCurrDate() >= test.testDate && getCurrDate() <= addMinToDate(test.testDate, test.testDuration)
                                    ?
                                    <>
                                      <Row><Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={() => handleBeginTest(test.testId)}>Începe testul</Button></Row>
                                      <Row><Button type='button' disabled className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={handleVisualizeResults(test.testId)}>Vizualizează rezultatele</Button></Row>
                                    </>
                                    :
                                    <>
                                      <Row><Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={() => handleBeginTest(test.testId)}>Începe testul</Button></Row>
                                      <Row><Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={handleVisualizeResults(test.testId)}>Vizualizează rezultatele</Button></Row>
                                    </>
                              )
                            }
                          </Container>

                        </td>
                      </tr>
                    ))
                  }
                </>
                : <tr>Nu e niciun test disponibil.</tr>
            }
          </tbody>
        </Table>
      </Row>
    </Container >
  )
}