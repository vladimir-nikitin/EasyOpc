import * as ReactDOM from 'react-dom';
import * as React from 'react';
import { App } from './components/App/App';
import { Provider } from 'react-redux';
import { configureStore } from './store/store';

ReactDOM.render(<Provider store={configureStore()}>
    <App />
  </Provider>, document.getElementById('root'));