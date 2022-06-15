import axios from "axios";

export const BASE_URL = 'http://localhost:5242';

//in order to avoid typos in components requests
export const ENDPOINTS = {
    login: 'users/login',
    register: 'users/register',
    courses: 'courses',
    subjects: 'subjects',
    categories: 'categories',
    questions: 'questions',
    tests: 'tests'
}

export const createAPIEndpoint = endpoint => {

    let url = BASE_URL + '/' + endpoint;

    let token = '';
    let config = {};

    if (localStorage.getItem('user')) {
        token = JSON.parse(localStorage.getItem('user')).token;

        config = {
            headers: { Authorization: `Bearer ${token}` }
        };
    }

    return {
        fetch: () => axios.get(url), //to retrieve all of the records
        fetchById: id => axios.get(url + '/' + id), //to retrieve one specific record
        post: newRecord => axios.post(url, newRecord),
        put: (id, updateRecord) => axios.put(url + '/' + id, updateRecord),
        delete: id => axios.delete(url + '/' + id),
        authFetch: () => axios.get(url, config), //to retrieve all of the records
        authFetchById: id => axios.get(url + '/' + id, config), //to retrieve one specific record
        authFetchByParams: params => axios.get(url, { ...config, params }),
        authPost: newRecord => axios.post(url, newRecord, config),
        authPut: (id, updateRecord) => axios.put(url + '/' + id, updateRecord, config),
        authDelete: id => axios.delete(url + '/' + id, config)
        //they will make a request to the corresponding web api methods of the type post get etc
    }

    //put token in authorization header
}