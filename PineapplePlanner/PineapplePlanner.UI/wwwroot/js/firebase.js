import { initializeApp } from "https://www.gstatic.com/firebasejs/9.6.1/firebase-app.js";
import {
    getAuth,
    onAuthStateChanged,
    signInWithEmailAndPassword,
    signOut
} from "https://www.gstatic.com/firebasejs/9.6.1/firebase-auth.js";


const initializeFirebase = async () => {
    const response = await fetch("firebase-config.json");
    const firebaseConfig = await response.json();

    const app = initializeApp(firebaseConfig);
    const auth = getAuth(app);

    window.firebaseAuth = {
        login: async function (email, password) {
            try {
                const userCredential = await signInWithEmailAndPassword(auth, email, password);
                return ({
                    success: true,
                    user: {
                        email: userCredential.user.email,
                        uid: userCredential.user.uid,
                        token: await userCredential.user.getIdToken()
                    }
                });
            } catch (error) {
                return ({
                    success: false,
                    error: error.message
                });
            }
        },
        logout: async function () {
            try {
                await signOut(auth);
                return ({ success: true });
            } catch (error) {
                return ({ success: false, error: error.message });
            }
        },
        getCurrentUser: function () {
            const user = auth.currentUser;
            if (user) {
                return ({
                    email: user.email,
                    uid: user.uid
                });
            } else {
                return null;
            }
        },
        onAuthStateChanged: function (dotNetHelper) {
            onAuthStateChanged(auth, (user) => {
                if (user) {
                    dotNetHelper.invokeMethodAsync("OnAuthStateChanged", ({
                        isLoggedIn: true,
                        email: user.email,
                        uid: user.uid
                    }));
                } else {
                    dotNetHelper.invokeMethodAsync("OnAuthStateChanged", ({
                        isLoggedIn: false
                    }));
                }
            });
        }
    };
};

//window.signInWithGoogle = () => {
//    signInWithRedirect(auth, provider);
//}

//window.handleRedirectResult = async () => {
//    try {
//        const result = await getRedirectResult(auth);
//        if (result && result.user) {
//            const idToken = await result.user.getIdToken();
//            return idToken;
//        }
//        return null;
//    } catch (error) {
//        console.error("Error handling redirect result:", error);
//        return null;
//    }
//}

//window.initAuthStateListener = (dotNetObjRef) => {
//    onAuthStateChanged(auth, async (user) => {
//        if (user) {
//            const idToken = await user.getIdToken();
//            dotNetObjRef.invokeMethodAsync("OnAuthStateChanged", idToken);
//        } else {
//            dotNetObjRef.invokeMethodAsync("OnAuthStateChanged", null);
//        }
//    });
//}

await initializeFirebase();
