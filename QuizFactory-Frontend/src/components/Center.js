import { Grid } from '@mui/material' //similar to the Bootstrap grid system, the container for the grid, item - similar to a column
import React from 'react'

//xs=1, 1 column space
//whatever has been wrapped with Center can be accessed with the props parameter 
export default function Center(props){
    return (
        <Grid
            container
            direction="column"
            alignItems="center"
            justifyContent="center"
            sx={{ minHeight:'100vh' }}>
            <Grid item xs={1}>
                {props.children}
            </Grid>
        </Grid>
    )
}