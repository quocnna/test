// components/Post.js

import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Post extends Component {
    static propTypes = {
        post: PropTypes.shape({
            id: PropTypes.number,
            title: PropTypes.string,
            content: PropTypes.string
        }).isRequired,
        editPost: PropTypes.func.isRequired,
        deletePost: PropTypes.func.isRequired
    };

    render() {
        return (
            <div>
                <h2>
                    { this.props.post.title }
                </h2>
                <p>
                    { this.props.post.content }
                </p>
                <button onClick={ () => this.props.editPost(this.props.post.id) }>
                    Edit
                </button>
                <button onClick={ () => this.props.deletePost(this.props.post.id) }>
                    Delete
                </button>
            </div>
        );
    }
}

export default Post;
