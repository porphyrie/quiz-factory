import React, { createContext, useContext, useEffect, useState } from 'react'

export const stateContext = createContext();

//in order to initialize this context object
const getFreshContext = () => {
    //check if we have a local storage for the item
    //pass a key to uniquely identify the data that we have saved + a json string corresponding to the data
    if (localStorage.getItem('context') === null)
        localStorage.setItem('context', JSON.stringify({
            participantId: 0,
            timeTaken: 0,
            selectedOptions: []
        }))
    //return an object with the properties that we want to save inside the context with their default values
    //return {
    //participantId: 0,
    //timeTaken: 0,
    //selectedOptions: []
    //}
    return JSON.parse(localStorage.getItem('context'));
}

export default function useStateContext() {
    const { context, setContext } = useContext(stateContext);

    return {
        context,
        setContext: obj => { setContext({ ...context, ...obj }) },
        resetContext: () => {
            //remove the context from localStorage
            localStorage.removeItem('context')
            //reset the state object context
            setContext(getFreshContext())
        }
    }
}

//?not exported as default
export function ContextProvider({ children }) {
    //inside this context object we will be saving the data that we want to share across these components here
    //so the data of this context is saved inside this state object
    //and it is shared across this children of the context provider
    const [context, setContext] = useState(getFreshContext());

    //update the localSorage when context is set
    useEffect(() => {
        localStorage.setItem('context', JSON.stringify(context))
    }, [context]);

    return (
        //we will pass this value to this context api here
        <stateContext.Provider value={{ context, setContext }}>
            {children}
        </stateContext.Provider>
    )
}