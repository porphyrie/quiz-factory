import { useState } from 'react';

export default function useForm(getFreshModelObject) { //a component with 2 state objects: values, errors, the values from textfields will be saved in values, the corresponding textfield errors in errors

    const [values, setValues] = useState(getFreshModelObject()); //initialized with aan object returned from this function passed as a parameter to this hook
    const [errors, setErrors] = useState({});//initialized with an empty object

    const handleInputChange = e => { //we don't have to rewrite the function in all the components where we have the form
        const { name, value } = e.target
        setValues({
            ...values,
            [name]: value //dynamically set the key in an object (wrap the object's key in array brackets)
        })
    }

    return {
        values,
        setValues,
        errors,
        setErrors,
        handleInputChange
    }
}