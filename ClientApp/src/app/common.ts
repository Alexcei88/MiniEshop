import { environment } from '../environments/environment';

let env = environment;


export const createImagePath = (serverPath: string) => {
    if(serverPath == null) {
        return 'assets/twitter-logo.png';
    }
    else{
        return `${env.server}${serverPath}`;
    }
  }
