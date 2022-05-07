import React from 'react'
import { Navigate, Outlet } from 'react-router-dom'
import useStateContext from '../hooks/useStateContext'

export default function Authenticate() {

    const { context } = useStateContext()

    //we will specify the targeted route where we want to redirect the unauthenticated participants - the default route or evrth should happen as per the route
    return (
        context.participantId == 0
            ? <Navigate to="/" />
            : <Outlet />
    )
}