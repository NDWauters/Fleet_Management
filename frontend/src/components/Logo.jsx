import logo from '../afbeeldingen/allphi.png';
import React from 'react';

const Logo = () => {
  return (
    <div className="mb-5 text-center">
      <a target={"_blank"} rel="noreferrer" href={"https://www.allphi.eu/"}>
        <img src={logo} alt="logo" />
      </a>
    </div>
  );
};

export default Logo;
