import { Breadcrumb } from "react-bootstrap";
import Navigation from "./home/Navigation";
import React from "react";
import LogoutButton from "./LogoutButton";

const ContextHeader = ({breadCrumbs}) => {

    return(
        <div className="row bg-dark">
            <div className="d-flex flex-row justify-content-between">
                <Breadcrumb className="breadcrumbs ms-2 link-light text-white">
                    {
                        breadCrumbs.map((breadCrumb, index) => {
                            return (
                                <Breadcrumb.Item className="link-light text-white" key={index} href={breadCrumb.link} active={breadCrumb.active}>
                                    {breadCrumb.name}
                                </Breadcrumb.Item>);
                        })
                    }
                </Breadcrumb>
                <Navigation size="2x"/>
                <LogoutButton />
            </div>
        </div>
    );
}

export default ContextHeader;
