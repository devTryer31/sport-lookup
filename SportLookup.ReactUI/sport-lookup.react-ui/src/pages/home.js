import React, { useState } from 'react'
import { signoutRedirect } from '../services/userService'
import { useSelector } from 'react-redux'
import * as apiService from '../services/apiService'
import { prettifyJson } from '../utils/jsonUtils'

function Home() {
  const user = useSelector(state => state.auth.user)
  const [doughnutData, setApiData] = useState(null)
  function signOut() {
    signoutRedirect()
  }

  async function getData() {
    const doughnuts = await apiService.getDataFromApi()
    setApiData(doughnuts)
  }

  return (
    <div>
      <h1>Home</h1>
      <p>Hello, {user.profile.given_name}.</p>
      <p>It is the token to call API </p>

      <button className="button button-outline" onClick={() => getData()}>Get API data</button>
      <button className="button button-clear" onClick={() => signOut()}>Sign Out</button>

      <pre>
        <code>
          {prettifyJson(doughnutData ? doughnutData : 'cannot get data')}
        </code>
      </pre>
    </div>
  )
}

export default Home
