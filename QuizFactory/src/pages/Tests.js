import React from 'react'
import Navigation from '../components/Navigation'
import { useState } from 'react'
import { parse } from 'postcss'
import { Link, useNavigate } from 'react-router-dom'
import { Button, Container, Row, Table } from 'react-bootstrap'

export default function Tests() {

  const [data, setData] = useState([
    {
      "testName": "Structuri de date și algoritmi",
      "dateTime": "Wed Apr 11 2018 00:50:44 GMT+0530 (India Standard Time)",
      "testLength": "90"
    },
    {
      "testName": "Arhitecturi paralele",
      "dateTime": "Wed Apr 11 2018 00:50:44 GMT+0530 (India Standard Time)",
      "testLength": "30"
    },
    {
      "testName": "Sisteme tolerante la defecte",
      "dateTime": "Wed Apr 11 2018 00:50:44 GMT+0530 (India Standard Time)",
      "testLength": "120"
    }
  ])

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

  const handleBeginTest = () => {
    navigate("/question");
  }

  const handleVisualizeResults = () => {
    navigate("/testresults");
  }

  const handleVisualizeDetails = () => {
    navigate("/test");
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
              <th>Denumire</th>
              <th>Dată</th>
              <th>Durată</th>
              <th>Nr. itemi</th>
            </tr>
          </thead>
          <tbody>
            {data.map((data) => (
              <tr>
                <td>{data.testName}</td>
                <td>{formatDate(data.dateTime)}</td>
                <td>{data.testLength} min</td>
                <td>x</td>
                <td>
                  <Container className='space-y-1'>
                    {getUserType() === 'profesor'
                      ? (<Row><Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={handleVisualizeDetails}>Detalii</Button></Row>)
                      : (
                        getCurrDate() < data.dateTime
                          ?
                          <>
                            <Row><Button type='button' disabled className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={handleBeginTest}>Începe testul</Button></Row>
                            <Row><Button type='button' disabled className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={handleVisualizeResults}>Vizualizează rezultatele</Button></Row>
                          </>
                          :
                          getCurrDate() >= data.dateTime && getCurrDate() <= addMinToDate(data.dateTime, data.testLength)
                            ?
                            <>
                              <Row><Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={handleBeginTest}>Începe testul</Button></Row>
                              <Row><Button type='button' disabled className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={handleVisualizeResults}>Vizualizează rezultatele</Button></Row>
                            </>
                            :
                            <>
                              <Row><Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={handleBeginTest}>Începe testul</Button></Row>
                              <Row><Button type='button' className='bg-violet-900 hover:bg-violet-600 border-violet-900 text-white' onClick={handleVisualizeResults}>Vizualizează rezultatele</Button></Row>
                            </>
                      )
                    }
                  </Container>
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      </Row>
    </Container >
  )
}