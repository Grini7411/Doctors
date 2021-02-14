const isActiveEl = document.getElementById('isActiveCheckbox');
const isPayingEl = document.getElementById('isPayingCheckbox');
let eventListenersToRemove = [];
window.addEventListener('load', () => {
    getDoctors();
    isActiveEl.addEventListener('change', getDoctors);
    eventListenersToRemove.push({element: isActiveEl, callback: getDoctors, listener: 'change'});
    isPayingEl.addEventListener('change', getDoctors);
    eventListenersToRemove.push({element: isPayingEl, callback: getDoctors, listener: 'change'});

    document.getElementById('contactModal').addEventListener('show.bs.modal', (event) => {
        const button = event.relatedTarget;
        const recipient = button.getAttribute('data-bs-docId');
        sessionStorage.setItem('docId', recipient)
        document.getElementById('contactModal').querySelector('#sendMsgBtn').addEventListener('click', sendMessage)
    })
})
window.addEventListener('beforeunload', () => {
    eventListenersToRemove.forEach(evLis => {
        evLis.element.removeEventListener(evLis.listener, evLis.callback);
    })
})

async function getDoctors() {
    const isActive = isActiveEl.checked;
    const isPaying = isPayingEl.checked;
    const templateEl = document.getElementById('doctorTemp');

    const url = "http://localhost:5000/api/doctor/get";
    await axios.get(url, { params: {isActive, isPaying} })
        .then(res => {
            document.getElementById('target').innerHTML = "";
            res.data.forEach(doctor => {
                const clone = templateEl.content.cloneNode(true);
                clone.querySelector('.docName').textContent = doctor.fullName;
                const starsContainer = clone.querySelector('.stars');
                for (let i = 0; i < doctor.reviews.averageRating; i++) {
                    starsContainer.innerHTML += `<span class="fa fa-star checked"></span>`;
                }
                while (starsContainer.children.length < 5) {
                    starsContainer.innerHTML += `<span class="fa fa-star"></span>`;
                }
                doctor.languageIds.forEach((lang, i) => {
                    if (i > 0) {
                        clone.querySelector('.languages').innerText += `, ${lang}`;
                    }
                    else{
                        clone.querySelector('.languages').innerText = `${lang}`;
                    }
                })
                clone.querySelector('.isArticle').innerText = doctor.hasArticle ? 'כן' : 'לא';
                clone.querySelector('.tel-btn').innerText = doctor.phones[0].number;
                clone.querySelector('.contact-btn').setAttribute('data-bs-docId', doctor.id);

                document.getElementById('target').appendChild(clone);
            })
        })
}

async function sendMessage() {
    const url = "http://localhost:5000/api/doctor";
    const dataJSON = {
        doctorId: +sessionStorage.getItem('docId'),
        fullName: document.getElementById('fullName').value,
        email: document.getElementById('email').value,
        phoneNumber: document.getElementById('phone').value
    }
    await axios.post(url, dataJSON)
        .then(res => {
            if (res.success){
                alert('sent successfully')
                document.getElementById('contactModal').hide();
            }
        })
}
