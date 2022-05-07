import { Box, Card, CardContent, CardHeader, CardMedia, LinearProgress, List, ListItemButton, Typography } from '@mui/material'
import React, { useContext } from 'react'
import { useState } from 'react'
import { useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { BASE_URL, createAPIEndpoint, ENDPOINTS } from '../api'
import { getFormatedTime } from '../helper'
import useStateContext, { stateContext } from '../hooks/useStateContext'

export default function Quiz() {

    //the context object contains the values from the context api and this set function can be used to update the values inside the context here  
    //stateContext - context api object
    //const { context, setContext } = useStateContext();
    //console.log(context);

    //in order to store the questions returned from the server
    const [ qns, setQns ] = useState([]);
    const [ qnIndex, setQnIndex ] = useState(0);
    const [ timeTaken, setTimeTaken ] = useState(0);
    const { context, setContext } = useStateContext();
    const navigate = useNavigate();

    let timer;

    const startTimer = () => {
        //increment timeTaken, it represents the seconds elapsed
        //whenever we create or configure a set interval operation here we need to suspend this operation => a reference to timer
        //
        setInterval(() => {
            setTimeTaken(prev => prev + 1);
        },[1000])
    }


    //this callback will be executed when this component is fully loaded
    //this callback function is invoken once the component is completely loaded
    //this hook represents component did mount event, with empty array parameter
    useEffect(() => {
        setContext({
            timeTaken: 0,
            selectedOptions: []
        })
        createAPIEndpoint(ENDPOINTS.question)
            .fetch()
            .then(res => {
                setQns(res.data);
                //start the timer as soon as we got the question from the server
                startTimer();
            })
            .catch(err => { console.log(err); })

        //if we return a function here, this function will get invoken whenever we leave this component for example, navigating to next component result or reloading the application
        //the perfect place to suspend the timer that we have created
        return () => {clearInterval(timer)}
    }, [])

    const updateAnswer = (qnId, optionIdx) => {
        const temp = [...context.selectedOptions]
        temp.push({
            qnId,
            selected: optionIdx
        })

        if (qnIndex < 4) {
            setContext({ selectedOptions: [...temp] })
            setQnIndex(qnIndex + 1)
        }
        else {
            setContext({ selectedOptions: [...temp], 
                timeTaken
            })
            navigate("/result")//navigate result component
        }
    }

    //we will show the questions if the length of the array is not equal to zero
    //disableRippleEffect
    //we are rendering the listitembutton component in a loop, so we have to set the key property with a unique value
    return (
        qns.length != 0
            ? <Card sx={{
                maxWidth: 640, mx: 'auto', mt: 5,
                '& .MuiCardHeader-action': { m: 0, alignSelf: 'center' }
            }}>
                <CardHeader
                    title={'Question ' + (qnIndex + 1) + ' of 5'}
                    action={<Typography>{getFormatedTime(timeTaken)}</Typography>}
                />
                <Box>
                    <LinearProgress variant="determinate" value={(qnIndex+1)*100/5}/>
                </Box>
                {qns[qnIndex].imageName != null
                    ? <CardMedia
                        component="img"
                        image={BASE_URL + 'images/' + qns[qnIndex].imageName}
                        sx={{ width: 'auto', m: '10px auto'}}
                      />
                    : null}
                <CardContent>
                    <Typography variant="h6">
                        {qns[qnIndex].qnInWords}
                    </Typography>
                    <List>
                        {qns[qnIndex].options.map((item, idx) =>
                            <ListItemButton
                                key={idx}
                                disableRipple
                                onClick={()=>updateAnswer(qns[qnIndex].qnId, idx)}
                            >
                                <div>
                                    <b>{String.fromCharCode(65 + idx) + "."}</b> {item}
                                </div>
                            </ListItemButton>
                        )}
                    </List>
                </CardContent>
              </Card> 
            :null
    )
}